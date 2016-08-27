
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Core;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.Media;
using AF.Services.Books;
using AF.Services.TiMus;
using AF.Web.Framework;
using AF.Web.Framework.UI;
using AF.Web.Models.Book;
using AF.Web.Models.Common;
using AF.Web.Models.Manager;

namespace AF.Web.Controllers
{
    public class DocuentMgrController : BasePublicController
    {
        private readonly IBookService _bookService;
        private readonly ISubjectService _subjectService;
        private readonly IBookChapterService _bookChapterService;
        private readonly IWebHelper _webHelper; 
        public DocuentMgrController(IBookService bookService, ISubjectService subjectService, IBookChapterService bookChapterService, IWebHelper webHelper)
        {
            this._bookService = bookService;
            _subjectService = subjectService;
            _bookChapterService = bookChapterService;
            _webHelper = webHelper;
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
            model.GradeItemList = gradeList.ToSelectItems();
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


        /// <summary>
        /// Indexes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
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


        /// <summary>
        /// Chapters the specified book identifier.
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <returns></returns>
        public ActionResult Chapter(int bookId)
        {
            var chapters = _bookChapterService.GetBookChapters(bookId);
            return View(chapters);
        }




        /// <summary>
        /// Chapters the upload.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChapterUpload(int chapterId)
        {
            var file = Request.Files[0];
            var chapter = _bookChapterService.GetBookChapterById(chapterId);
            if (null == chapter || file == null || file.ContentLength == 0) return Json(false);

            var extens = Path.GetExtension(file.FileName);
            var filename = Guid.NewGuid() + extens;
            var relatepath = $"/Content/Upload/World/{filename}";
            var phyPath = _webHelper.MapPath("~" + relatepath);
            file.SaveAs(phyPath);
            var entity = new AF.Domain.Domain.Media.File()
            {
                OriginallName = file.FileName,
                CreateTime = DateTime.Now,
                Name = filename,
                RelatePath = relatepath
            };
            chapter.UploadFile = entity;
            _bookChapterService.UpdateBookChapter(chapter);
            return Json(true);
        }


    }
}