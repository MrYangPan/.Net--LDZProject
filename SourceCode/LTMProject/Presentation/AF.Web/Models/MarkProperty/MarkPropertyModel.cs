using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Domain.Domain.Books;
using AF.Services.TiMus.TiMuCompose;

namespace AF.Web.Models.MarkProperty
{
    public class MarkPropertyModel
    {
        public MarkPropertyModel() 
        {
            BookTimus=new List<BookTiMu>();
        }

        public IList<BookTiMu> BookTimus;

        public int TaskId { get; set; }

        public int TaskItemId { get; set; }

        public Guid Tmid { get; set; }

        public TiMuModel TiMuModel { get; set; }

        /// <summary>
        /// 所有相关目录.
        /// </summary>
        /// <value>
        /// The book chapters.
        /// </value>
        public IList<BookChapter> RelateAllChapters { get; set; }

        /// <summary>
        /// Gets or sets the book ti mu.
        /// </summary>
        /// <value>
        /// The book ti mu.
        /// </value>
        public BookTiMu BookTiMu { get; set; }
    }
}