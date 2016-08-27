using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.TiMus;
using AF.Services.Books;
using AF.Services.BookWork;
using AF.Services.TiMus;
using AF.Web.Framework;
using AF.Web.Framework.UI;
using AF.Web.Models.Book;
using AF.Web.Models.Common;
using AF.Web.Models.Manager;

namespace AF.Web.Controllers
{
    public class ManageController : BasePublicController
    {
        private readonly IBookService _bookService;
        private readonly ISubjectService _subjectService;
        private readonly IBookChapterService _bookChapterService;
        private readonly IBookWorkTaskService _bookWorkTaskService;
        public ManageController(IBookService bookService, ISubjectService subjectService, IBookChapterService bookChapterService,
            IBookWorkTaskService bookWorkTaskService)
        {
            _bookService = bookService;
            _subjectService = subjectService;
            _bookChapterService = bookChapterService;
            _bookWorkTaskService = bookWorkTaskService;
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

            if (model.DegreeId > 0)
            {
                var gradeList = _bookService.GetGradeList(model.DegreeId);
                model.GradeItemList = gradeList.ToSelectItems();
            }
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


        public ActionResult Index(BookSearchModel model, PagingFilteringModel command)
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

        public ActionResult EditBook(int id)
        {
            var book = _bookService.GetBookById(id);
            var bookModel = new SetBookModel()
            {
                Id = book.Id,
                Isbn = book.Isbn,
                Name = book.Name,
                DegreeId = book.DegreeId,
                GradeId = book.GradeId,
                SubjectId = book.SubjectId,
                Year = book.Year,
                PublisherId = book.PublisherId,
                TermId = book.TermId
            };
            PrepareBookModel(bookModel);
            return View(bookModel);
        }

        [HttpPost]
        public ActionResult EditBook(SetBookModel model)
        {
            if (!ModelState.IsValid)
            {
                PrepareBookModel(model);
                return View(model);
            }
            var book = _bookService.GetBookById(model.Id);
            book.SubjectId = model.SubjectId;
            book.DegreeId = model.DegreeId;
            book.GradeId = model.GradeId;
            book.TermId = model.TermId;
            book.Year = model.Year;
            book.Isbn = model.Isbn;
            book.Name = model.Name;
            book.PublisherId = model.PublisherId;
            book.SubjectId = model.SubjectId;
            book.Version = model.Version;
            _bookService.UpdateBook(book);
            return RedirectToAction("Index");
        }

        public ActionResult SetBook()
        {
            var param = new SetBookViewModel();
            PrepareBookModel(param);
            var paramList = new List<SetBookViewModel> { param };
            return View(paramList);
        }

        [HttpPost]
        public JsonResult SetBook(List<SetBookViewModel> list)
        {
            if (list == null || list.Count == 0)
            {
                return Json(new { state = false });
            }
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.Name) || item.SubjectId == 0)
                {
                    return Json(new { state = false });
                }

                _bookService.InsertBook(new Book()
                {
                    SubjectId = item.SubjectId,
                    DegreeId = item.DegreeId,
                    GradeId = item.GradeId,
                    TermId = item.TermId,
                    Year = item.Year,
                    Isbn = item.Isbn,
                    Name = item.Name,
                    PublisherId = item.PublisherId,
                    CreateTime = DateTime.Now,
                    Version = item.Version
                });
            }
            return Json(new { state = true });
        }

        public ActionResult DeleteBook(int id)
        {
            var model = _bookService.GetBookById(id);
            if (model != null)
            {
                _bookService.DeleteBook(model);


            }
            return RedirectToAction("Index");
        }

        public JsonResult GetGrade(int degreeId)
        {
            var gradeList = _bookService.GetGradeList(degreeId);
            return Json(gradeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubject(int degreeId)
        {
            IList<Subject> list;
            if (degreeId == 0)
                list = _subjectService.GetSubjectList();
            else
                list = _subjectService.GetSubjectList(degreeId);
            return Json(list.Select(t => new { Id = t.Id, Name = t.Name }), JsonRequestBehavior.AllowGet);
        }

        #region Chapter Action

        public ActionResult Chapter(int bookId)
        {
            var chapters = _bookChapterService.GetBookChapters(bookId);
            ViewBag.bookId = bookId;
            return View(chapters);
        }

        [HttpPost]
        public JsonResult UpdateChapter(UpdateChapterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(false);
            }

            BookChapter chapter;
            if (model.Id == 0)
            {
                chapter = new BookChapter
                {
                    Name = model.Name,
                    IsLock = false,
                    BookId = model.BookId
                };
                if (model.ParentId != 0)
                    chapter.ParentId = model.ParentId;

                _bookChapterService.InsertBookChapter(chapter);
            }
            else
            {
                chapter = _bookChapterService.GetBookChapterById(model.Id);
                chapter.Name = model.Name;
                chapter.BookId = model.BookId;
                _bookChapterService.UpdateBookChapter(chapter);
            }
            return Json(true);
        }


        public ActionResult DeleteChapter(int bookId, int chapterId)
        {
            var chapter = _bookChapterService.GetBookChapterById(chapterId);
            int? parentId = chapter?.BookChapterParent?.Id;
            if (chapter != null)
            {
                _bookChapterService.DeleteBookChapter(chapter.BookChapterChild.ToArray());
                _bookChapterService.DeleteBookChapter(chapter);
            }
            return RedirectToAction("Chapter", new { bookId });
        }

        public ActionResult LockOrUnlock(int bookId, int chapterId, bool type)
        {
            var chapter = _bookChapterService.GetBookChapterById(chapterId);
            if (chapter != null)
            {
                chapter.IsLock = type;
                _bookChapterService.UpdateBookChapter(chapter);
            }
            return RedirectToAction("Chapter", new { bookId });
        }

        #endregion

    }
}