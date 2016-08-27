using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Core.Data;
using AF.Core.Extensions;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;
using AF.Domain.Domain.Customers;
using AF.Domain.Domain.Knowledge;
using AF.Domain.Domain.TiMus;
using AF.Domain.Infrastructure;
using AF.Services.Books;
using AF.Services.BookWork;
using AF.Services.TiMus;
using AF.Services.TiMus.TiMuCompose;
using AF.Web.Framework.UI;
using AF.Web.Models.CheckPublish;

namespace AF.Web.Controllers
{
    public class CheckPublishController : BasePublicController
    {
        #region Fields
        private readonly IWorkContext _workContext;
        private readonly IBookWorkTaskService _bookWorkTaskService;
        private readonly IBookWorkTaskItemService _bookWorkTaskItemService;
        private readonly ISubjectService _subjectService;
        private readonly ITiMuService _tiMuService;
        private readonly IBookChapterService _bookChapterService;
        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckPublishController" /> class.
        /// </summary>
        /// <param name="workContext">The work context.</param>
        /// <param name="bookWorkTaskService">The book work task service.</param>
        /// <param name="subjectService">The subject service.</param>
        /// <param name="bookWorkTaskItemService">The book work task item service.</param>
        /// <param name="tiMuService">The ti mu service.</param>
        /// <param name="bookChapterService">The book chapter service.</param>
        public CheckPublishController(IWorkContext workContext, IBookWorkTaskService bookWorkTaskService,
            ISubjectService subjectService, IBookWorkTaskItemService bookWorkTaskItemService, ITiMuService tiMuService, IBookChapterService bookChapterService)
        {
            this._workContext = workContext;
            _bookWorkTaskService = bookWorkTaskService;
            _subjectService = subjectService;
            _bookWorkTaskItemService = bookWorkTaskItemService;
            _tiMuService = tiMuService;
            _bookChapterService = bookChapterService;
        }

        #endregion

        // GET: CheckPublish
        public ActionResult CheckTaskList(CheckTaskListModel model, CheckTaskListPagingFilteringModel command)
        {
            if (command.PageNumber <= 0)
                command.PageNumber = 1;
            if (null == model)
                model = new CheckTaskListModel();
            var subject = _subjectService.GetSubjectList();
            model.SubjectList.Add(new SelectListItem() { Text = "请选择", Value = string.Empty });
            subject.ToSelectItems().ForEach(t => model.SubjectList.Add(t));
            if (_workContext.CurrentCustomer.IsAdmin())
            {
                var bookTasks = _bookWorkTaskService.GetAllTasks(taskStatus: TaskStatus.Check, subjectId: model.SubjectId,
                    pageIndex: command.PageNumber - 1, pageSize: command.PageSize);
                model.BookWorkTaskList = bookTasks;
                model.PagingFilteringContext.LoadPagedList(bookTasks);
            }
            else
            {
                var bookTasks = _bookWorkTaskService.GetAllTasks(_workContext.CurrentCustomer.Id, TaskStatus.Check,
                    model.SubjectId, command.PageNumber - 1, command.PageSize);
                model.BookWorkTaskList = bookTasks;
                model.PagingFilteringContext.LoadPagedList(bookTasks);
            }

            return View(model);
        }

        public ActionResult CheckTaskDetail(int taskId)
        {
            var model = new CheckTaskDetailModel {TaskId = taskId};
            var booktask = _bookWorkTaskService.GetBookWorkTask(taskId);
            var taskItems = booktask.BookWorkTaskItems.ToList(); //任务包含的Item
            model.TaskRelatedChapter = taskItems.Select(d => d.BookChapter).ToList();
            IList<BookChapter> bookChapterList = new List<BookChapter>();
            foreach (var titem in taskItems)
            {
                var itemchapter = titem.BookChapter;
                while (itemchapter.BookChapterParent != null)
                {
                    itemchapter = itemchapter.BookChapterParent;
                }
                if (!bookChapterList.Contains(itemchapter))
                {
                    bookChapterList.Add(itemchapter);
                }
            }
            bookChapterList = bookChapterList.OrderBy(p => p.Id).ToList();
            model.RelateChapters = bookChapterList;
            return View(model);
        }

        public ActionResult CheckTaskProperty(int taskitemId, int taskId)
        {
            var model = new CheckTaskPropertyModel {TaskItemId = taskitemId, TaskId = taskId};
            var booktimus = _bookWorkTaskItemService.GetBookTiMus(taskitemId);
            var task = _bookWorkTaskService.GetBookWorkTask(taskId);
            var taskitem = _bookWorkTaskItemService.GetBookWorkTaskItemsById(taskitemId);
            model.BookTimus = booktimus;
            model.Task = task;
            model.TaskItem = taskitem;
            var checkTiMuModels = new List<CheckTiMuModel>();
            foreach (var booktimu in booktimus)
            {
                var checkTiMuModel = new CheckTiMuModel();
                var timu = _tiMuService.GeTiMuById(booktimu.Id);
                var tbbr = new TiMuBuiltRequest(tiMu: timu);
                var tmbuder = new TiMuBuild(tbbr);
                var tiMuModel = tmbuder.AssemblyTimu();
                checkTiMuModel.TiMuModel = tiMuModel;
                checkTiMuModel.LargeNumber = booktimu.LargeNumber;
                checkTiMuModel.SmallNumber = booktimu.SmallNumber;
                checkTiMuModel.PageNumber = booktimu.PageNumber;
                checkTiMuModels.Add(checkTiMuModel);
            }
            model.TiMuModels = checkTiMuModels;
            var taskItem = _bookWorkTaskItemService.GetBookWorkTaskItemsById(taskitemId);
            model.RelateAllChapters = _bookChapterService.GetParentListById(taskItem.ChapterId);
            return View(model);
        }

        public ActionResult SubmitTask(int taskId)
        {
            var task = _bookWorkTaskService.GetBookWorkTask(taskId);
            task.Status = TaskStatus.Complete;
            _bookWorkTaskService.SubmitTask(task);
            return RedirectToAction("CheckTaskList");
        }

        public ActionResult RevertTask(int taskId)
        {
            var task = _bookWorkTaskService.GetBookWorkTask(taskId);
            task.Status = TaskStatus.Revert;
            _bookWorkTaskService.SubmitTask(task);
            return RedirectToAction("CheckTaskList");
        }

        public JsonResult SignWrong(Guid tmid, BookTiMu.TimuStatus status, int taskId, string errorInfo)
        {
            var booktimu = _bookWorkTaskItemService.GetBookTiMuById(tmid);
            if (booktimu != null)
                booktimu.Status = status; booktimu.ErrorInfo = errorInfo;
            _bookWorkTaskService.SignTmWrong(booktimu);//标记错题

            //标记章节错题状态
            var chapterCount= booktimu.TaskItem.TaskBookTiMus.Count(p => p.Status == BookTiMu.TimuStatus.Invalid);
            booktimu.TaskItem.Status = chapterCount > 0 ? 1 : 0;
            _bookWorkTaskItemService.UpdateBookWorkTaskItem(booktimu.TaskItem);

            var task = _bookWorkTaskService.GetBookWorkTask(taskId);//任务
            var alltasktimu = _bookWorkTaskService.GetTaskBookTiMus(taskId);//任务包含的题目
            var wrongcount = alltasktimu.Count(p => p.Status == BookTiMu.TimuStatus.Invalid);//任务包含标记有误的题目数
            var valid = 1;
            if (wrongcount > 0)
                valid = 0;
            task.AllTmIsvalid = valid;
            _bookWorkTaskService.SubmitTask(task);
            if (status == BookTiMu.TimuStatus.Valid)
            {
                object result = new { IsWrong = false, Status = status };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                object result = new { IsWrong = true, Status = status };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}