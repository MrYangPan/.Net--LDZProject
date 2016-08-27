using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;
using AF.Services.TiMus.TiMuCompose;

namespace AF.Web.Models.EntryExamination
{
    /// <summary>
    /// 题目预览Model
    /// </summary>
    public class EntryTiMuModel
    {
        public string Tmid { get; set; }

        public IList<TiMuModel> TiMulIsList { get; set; }

        public EntryExaminationListPagingModel EntryExaminationListPagingModel { get; set; }

        /// <summary>
        /// 子父章节集合
        /// </summary>
        /// <value>
        /// The book chapter.
        /// </value>
        public IList<BookChapter> BookChapterList { get; set; }

        /// <summary>
        /// Gets or sets the book work task.
        /// </summary>
        /// <value>
        /// The book work task.
        /// </value>
        public BookWorkTask BookWorkTask { get; set; }

        public EntryTiMuModel()
        {
            EntryExaminationListPagingModel = new EntryExaminationListPagingModel();
            TiMulIsList = new List<TiMuModel>();
        }
    }
}