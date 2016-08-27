using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;
using AF.Core.Extensions;
using AF.Domain.Infrastructure;
using AF.Services.BookWork;
using AF.Services.TiMus;
using AF.Web.Models.MarkProperty;
using AF.Domain.Domain.Customers;
using AF.Domain.Domain.BookWork;
using AF.Core.Data;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.Knowledge;
using AF.Domain.Domain.TiMus;
using AF.Services.Books;
using AF.Services.Knowledge;
using AF.Services.TiMus.TiMuCompose;
using AF.Web.Framework.UI;

namespace AF.Web.Controllers
{
    public class MarkPropertyController : BasePublicController
    {
        #region Fields
        private readonly IWorkContext _workContext;
        private readonly IBookWorkTaskService _bookWorkTaskService;
        private readonly IBookWorkTaskItemService _bookWorkTaskItemService;
        private readonly ISubjectService _subjectService;
        private readonly ITiMuService _tiMuService;
        private readonly IKnowledgeService _knowledgeService;
        private readonly IBookChapterService _bookChapterService;
        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkPropertyController" /> class.
        /// </summary>
        /// <param name="workContext">The work context.</param>
        /// <param name="bookWorkTaskService">The book work task service.</param>
        /// <param name="subjectService">The subject service.</param>
        /// <param name="bookWorkTaskItemService">The book work task item service.</param>
        /// <param name="tiMuService">The ti mu service.</param>
        /// <param name="knowledgeService">The knowledge service.</param>
        /// <param name="bookChapterService">The book chapter service.</param>
        public MarkPropertyController(IWorkContext workContext, IBookWorkTaskService bookWorkTaskService,
            ISubjectService subjectService, IBookWorkTaskItemService bookWorkTaskItemService,
            ITiMuService tiMuService, IKnowledgeService knowledgeService, IBookChapterService bookChapterService)
        {
            this._workContext = workContext;
            _bookWorkTaskService = bookWorkTaskService;
            _subjectService = subjectService;
            _bookWorkTaskItemService = bookWorkTaskItemService;
            _tiMuService = tiMuService;
            _knowledgeService = knowledgeService;
            _bookChapterService = bookChapterService;
        }

        #endregion

        public ActionResult MarkTaskList(MarkTaskListModel model, MarkTaskListPagingFilteringModel command)
        {
            if (command.PageNumber <= 0)
                command.PageNumber = 1;
            if (null == model)
                model = new MarkTaskListModel();
            var subject = _subjectService.GetSubjectList();
            model.SubjectList.Add(new SelectListItem() {Text = "请选择", Value = string.Empty});
            subject.ToSelectItems().ForEach(t => model.SubjectList.Add(t));
            if (_workContext.CurrentCustomer.IsAdmin())
            {
                var bookTasks = _bookWorkTaskService.GetAllTasks(taskStatus: TaskStatus.Mark, subjectId: model.SubjectId,
                    pageIndex: command.PageNumber - 1, pageSize: command.PageSize);
                model.BookWorkTaskList = bookTasks;
                model.PagingFilteringContext.LoadPagedList(bookTasks);
            }
            else
            {
                var bookTasks = _bookWorkTaskService.GetAllTasks(_workContext.CurrentCustomer.Id, TaskStatus.Mark,
                    model.SubjectId, command.PageNumber - 1, command.PageSize);
                model.BookWorkTaskList = bookTasks;
                model.PagingFilteringContext.LoadPagedList(bookTasks);
            }

            return View(model);
        }

        public ActionResult MarkTaskDetail(int taskId)
        {
            var model = new MarkTaskDetailModel();
            model.TaskId = taskId;
            var booktask = _bookWorkTaskService.GetBookWorkTask(taskId);
            var taskItems = booktask.BookWorkTaskItems.ToList(); //任务包含的Item
            model.TaskRelatedChapter = taskItems.Select(d => d.BookChapter).ToList();
            IList<BookChapter> bookChapterList=new List<BookChapter>();
            //IList<BookChapter> relateAllChapters = new List<BookChapter>();
            foreach (var titem in taskItems)
            {
                var itemchapter = titem.BookChapter;
                while (itemchapter.BookChapterParent != null)
                {
                    //if (!relateAllChapters.Contains(itemchapter))
                    //{
                    //    relateAllChapters.Add(itemchapter);
                    //}
                    itemchapter = itemchapter.BookChapterParent;
                }
                if (!bookChapterList.Contains(itemchapter))
                {
                    bookChapterList.Add(itemchapter);
                }
            }
            bookChapterList = bookChapterList.OrderBy(p => p.Id).ToList();
            model.RelateChapters = bookChapterList;
            //model.RelateAllChapters = relateAllChapters;
            return View(model);
        }

        public ActionResult MarkTaskProperty(int taskitemId, int taskId, Guid? tmid = null)
        {
            var model = new MarkPropertyModel {TaskId = taskId, TaskItemId = taskitemId};
            var timuList = _bookWorkTaskItemService.GetBookTiMus(taskitemId);
            model.BookTimus = timuList;//包含的题目
            //需要展示的题目
            model.Tmid = tmid ?? timuList.First().Id;
            var timu = _tiMuService.GeTiMuById(model.Tmid);
            model.BookTiMu = timu.BookTiMu;
            var tbbr = new TiMuBuiltRequest(tiMu: timu);
            var tmbuder = new TiMuBuild(tbbr);
            model.TiMuModel= tmbuder.AssemblyTimu();
            var taskItem = _bookWorkTaskItemService.GetBookWorkTaskItemsById(taskitemId);
            model.RelateAllChapters = _bookChapterService.GetParentListById(taskItem.ChapterId);
            return View(model);
        }

        public ActionResult MarkPreview(int taskitemId, int taskId)
        {
            var model = new MarkPreviewModel();
            var booktimus = _bookWorkTaskItemService.GetBookTiMus(taskitemId);
            var task = _bookWorkTaskService.GetBookWorkTask(taskId);
            var taskitem = _bookWorkTaskItemService.GetBookWorkTaskItemsById(taskitemId);
            model.BookTimus = booktimus;
            model.Task = task;
            model.TaskItem = taskitem;
            IList<TiMuModel> tiMuModels = new List<TiMuModel>();
            foreach (var booktimu in booktimus)
            {
                var tbbr = new TiMuBuiltRequest(tiMu: booktimu.Timu);
                var tmbuder = new TiMuBuild(tbbr);
                var tiMuModel= tmbuder.AssemblyTimu();
                tiMuModels.Add(tiMuModel);
            }
            model.TiMuModels = tiMuModels;
            var taskItem = _bookWorkTaskItemService.GetBookWorkTaskItemsById(taskitemId);
            model.RelateAllChapters = _bookChapterService.GetParentListById(taskItem.ChapterId);
            return View(model);
        }

        public JsonResult CheckTaskIsDone(int taskId)
        {
            var taskBookTimus = _bookWorkTaskService.GetTaskBookTiMus(taskId);
            var allcount = taskBookTimus.Count;
            var markcount = taskBookTimus.Count(p => p.Timu.Difficult > 0);
            if (allcount == markcount || _workContext.CurrentCustomer.IsAdmin())
            {
                var text = "提交后无法再进行修改，确认提交吗？";
                object result = new {IsDone = true, Text = text};
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var text = "共" + allcount + "题，目前只标定" + markcount + "题，无法提交";
                object result = new {IsDone = false, Text = text};
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SubmitTask(int taskId)
        {
            var booktask = _bookWorkTaskService.GetBookWorkTask(taskId);
            booktask.Status = TaskStatus.Check;
            _bookWorkTaskService.SubmitTask(booktask);
            return RedirectToAction("MarkTaskList");
        }
    }
}