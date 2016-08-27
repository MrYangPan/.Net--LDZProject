using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;
using AF.Services.TiMus.TiMuCompose;

namespace AF.Web.Models.MarkProperty
{
    public class MarkPreviewModel
    {
        public IList<BookTiMu> BookTimus;

        public BookWorkTask Task { get; set; }

        public BookWorkTaskItem TaskItem { get; set; }

        public IList<TiMuModel> TiMuModels { get; set; }

        /// <summary>
        /// 所有相关目录.
        /// </summary>
        /// <value>
        /// The book chapters.
        /// </value>
        public IList<BookChapter> RelateAllChapters { get; set; }
    }
}