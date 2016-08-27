using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Core;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;
using AF.Services.Books;
using AF.Services.BookWork;
using AF.Services.Customers;
using AF.Services.TiMus;
using AF.Web.Framework.UI;
using AF.Web.Models.Book;
using AF.Web.Models.Common;
using AF.Web.Models.Manager;
using AF.Web.Models.MarkProperty;
using AF.Web.Models.TaskAllocation;
using WebGrease.Css.Extensions;

namespace AF.Web.Controllers
{
    public class TaskAllocationController : BasePublicController
    {
        private readonly IBookService _bookService;
        private readonly IBookChapterService _bookChapterService;
        private readonly IBookWorkTaskService _bookWorkTaskService;
        private readonly IBookWorkTaskItemService _bookWorkTaskItemService;
        private readonly ISubjectService _subjectService;
        private readonly ICustomerService _customerService;

        public TaskAllocationController(IBookService bookService, IBookChapterService bookChapterService,
            IBookWorkTaskService bookWorkTaskService, IBookWorkTaskItemService bookWorkTaskItemService,
            ISubjectService subjectService, ICustomerService customerService)
        {
            _bookService = bookService;
            _bookChapterService = bookChapterService;
            _bookWorkTaskService = bookWorkTaskService;
            _bookWorkTaskItemService = bookWorkTaskItemService;
            _subjectService = subjectService;
            _customerService = customerService;
        }

        #region Unitilies


        /// <summary>
        /// Prepares the book model.
        /// </summary>
        /// <param name="model">The model.</param>
        private void PrepareBookModel(BookCommonModel model)
        {
            var degreeList = _bookService.GetDegreeList();
            model.DegreeItemList = degreeList.ToSelectItems();
            model.DegreeItemList.Insert(0, new SelectListItem() { Text = "请选择", Value = "0" });

            var gradeList = _bookService.GetGradeList();
            //model.GradeItemList = gradeList.ToSelectItems();
            model.GradeItemList.Insert(0, new SelectListItem() { Text = "请选择", Value = "0" });

            var subjectList = _subjectService.GetSubjectList();
            model.SubjectItemList = subjectList.ToSelectItems();
            model.SubjectItemList.Insert(0, new SelectListItem() { Text = "请选择", Value = "0" });

            var publishList = _bookService.GetAllPublishers();
            model.PublishItemList = publishList.ToSelectItems();
            model.PublishItemList.Insert(0, new SelectListItem() { Text = "请选择", Value = "0" });


            var termList = _bookService.GetAllTerms();
            model.TermItemList = termList.ToSelectItems();
            model.TermItemList.Insert(0, new SelectListItem() { Text = "请选择", Value = "0" });

            var yearList = _bookService.GetAllExaYears();
            model.YearItemList = yearList.ToSelectItems();
            model.YearItemList.Insert(0, new SelectListItem() { Text = "请选择", Value = "0" });
        }

        #endregion

        // GET: TaskAllocation
        public ActionResult Index(BookTaskModel model, PagingFilteringModel command)
        {
            PrepareBookModel(model);

            var request = new BookWorkRequest()
            {
                Name = model.Name,
                Begin = model.Begin,
                DegreeId = model.DegreeId,
                End = model.End,
                GradeId = model.GradeId,
                Isbn = model.Isbn,
                PublisherId = model.PublisherId,
                SubjectId = model.SubjectId,
                TermId = model.TermId,
                Year = model.Year,
                PageIndex = command.PageIndex,
                PageSize = 10
            };

            var pageData = _bookWorkTaskService.GetBookWorkList(request);
            model.BookWorkTaskList = pageData;
            model.PagingFilteringModel.LoadPagedList(pageData);
            return View(model);
        }

        public ActionResult EditTask(int id)
        {
            var task = _bookWorkTaskService.GetBookWorkTask(id);
            if (task == null)
                return RedirectToAction("Index");

            var model = new EditTaskModel();
            SetEditTaskParam(model, task);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditTask(EditTaskModel model)
        {
            var task = _bookWorkTaskService.GetBookWorkTask(model.TaskId);
            if (task == null) return RedirectToAction("Index");

            if (!ModelState.IsValid)
            {
                SetEditTaskParam(model, task);
                return View("EditTask", model);
            }
            task.EntryCustomerId = model.EntryCustomerId;
            task.MarkCustomerId = model.EntryCustomerId;
            task.CheckCustomerId = model.CheckCustomerId;
            task.Status = model.TaskStatus;
            _bookWorkTaskService.UpdateBookWorkTask(task);
            return RedirectToAction("Index");
        }

        public ActionResult BookList(BookSearchModel model, PagingFilteringModel command)
        {
            PrepareBookModel(model);
            var request = new BookRequest()
            {
                Name = model.Name,
                Begin = model.Begin,
                DegreeId = model.DegreeId,
                End = model.End,
                GradeId = model.GradeId,
                Isbn = model.Isbn,
                PublisherId = model.PublisherId,
                SubjectId = model.SubjectId,
                TermId = model.TermId,
                Year = model.Year,
                PageIndex = command.PageIndex,
                PageSize = 10
            };
            var pageData = _bookService.GetBookPage(request);
            model.BookList = pageData;
            model.PagingFilteringModel.LoadPagedList(pageData);
            return View(model);
        }

        public ActionResult Chapter(int bookId)
        {
            var task = new ChapterParamModel
            {
                BookId = bookId,
                BookChapterList = _bookChapterService.GetBookChapters(bookId)
            };
            return View(task);
        }

        [HttpPost]
        public ActionResult Chapter(ChapterParamModel param)
        {
            if (!ModelState.IsValid)
            {
                param.BookChapterList = _bookChapterService.GetBookChapters(param.BookId);
                return View(param);
            }
            var chapterIdArr = param.BookChapterIds.Split(',');
            //获取题目数量
            var bookChapterList = chapterIdArr.Select(t => _bookChapterService.GetBookChapterById(int.Parse(t))).ToList();

            //生成任务名
            string taskName = DateTime.Now.ToString("yyyyMMdd-1");
            var works = _bookWorkTaskService.GetBookWorkList(new BookWorkRequest() { Begin = DateTime.Now, End = DateTime.Now, PageSize = 1 });
            var work = works.FirstOrDefault();
            if (!string.IsNullOrEmpty(work?.Name))
            {
                int num = Convert.ToInt32(work.Name.ToCharArray().Last().ToString()) + 1;
                taskName = DateTime.Now.ToString("yyyyMMdd-" + num);
            }

            var model = new BookWorkTask()
            {
                Name = taskName,
                BookId = param.BookId,
                CheckCustomerId = param.CheckCustomerId,
                //标定 录入人
                MarkCustomerId = param.EntryCustomerId,
                EntryCustomerId = param.EntryCustomerId,
                Status = TaskStatus.Entry,
                CreateTime = DateTime.Now
            };
            _bookWorkTaskService.InsertBookWorkTask(model);

            var taskItemList = chapterIdArr.Select(t => new BookWorkTaskItem()
            {
                TaskId = model.Id,
                ChapterId = int.Parse(t),
                Status = 0
            }).ToList();
            _bookWorkTaskItemService.InsertBookWorkTaskItems(taskItemList);

            //修改章节表的TaskItemId
            taskItemList.ForEach(t =>
            {
                var bookChapter = bookChapterList.First(h => h.Id == t.ChapterId);
                bookChapter.TaskItemId = t.Id;
                _bookChapterService.UpdateBookChapter(bookChapter);
            });
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int taskId)
        {
            var model = new MarkTaskDetailModel { Id = taskId };
            var booktask = _bookWorkTaskService.GetBookWorkTask(taskId);
            var taskItems = booktask.BookWorkTaskItems.ToList(); //任务包含的Item
            model.TaskRelatedChapter = taskItems.Select(d => d.BookChapter).ToList();
            foreach (var titem in taskItems)
            {
                var itemchapter = titem.BookChapter;
                while (itemchapter.BookChapterParent != null)
                {
                    itemchapter = itemchapter.BookChapterParent;
                }
                if (!model.RelateChapters.Contains(itemchapter))
                {
                    model.RelateChapters.Add(itemchapter);
                }
            }

            return View(model);
        }

        public ActionResult DeleteBookWorkTask(int id)
        {
            var model = _bookWorkTaskService.GetBookWorkTask(id);
            if (model != null)
            {
                _bookWorkTaskService.DeleteBookWorkTask(model);
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetCustomer(string keyword)
        {
            var list = _customerService.GetAllCustormers(keyword, 0, null, null, null, pageIndex: 0, pageSize: 10);
            var dataList = list.Select(t => new { id = t.Id, text = t.Username }).ToList();
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        private void SetEditTaskParam(EditTaskModel model, BookWorkTask task)
        {
            if (task == null) return;

            model.StatusItemList.Add(new SelectListItem() { Text = "录入", Value = ((int)TaskStatus.Entry).ToString() });
            model.StatusItemList.Add(new SelectListItem() { Text = "标定", Value = ((int)TaskStatus.Mark).ToString() });
            model.StatusItemList.Add(new SelectListItem() { Text = "审核", Value = ((int)TaskStatus.Check).ToString() });
            model.StatusItemList.Add(new SelectListItem() { Text = "回撤", Value = ((int)TaskStatus.Revert).ToString() });
            model.StatusItemList.Add(new SelectListItem() { Text = "完成", Value = ((int)TaskStatus.Complete).ToString() });

            if (model.TaskId == 0)
            {
                model.TaskStatus = task.Status;
            }

            model.TaskId = task.Id;

            model.CheckCustomerName = model.CheckCustomerName ?? task.CheckCustomer.Username;
            model.EntryCustomerName = model.EntryCustomerName ?? task.EntryCustomer.Username;

            model.EntryCustomerId = model.EntryCustomerId ?? task.EntryCustomerId;
            model.CheckCustomerId = model.CheckCustomerId ?? task.CheckCustomerId;
            
        }
    }
}