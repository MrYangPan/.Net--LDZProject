using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;
using AF.Services.TiMus.TiMuCompose;

namespace AF.Web.Models.CheckPublish
{
    public class CheckTaskPropertyModel
    {
        public int TaskId { get; set; }

        public int TaskItemId { get; set; }

        public IList<BookTiMu> BookTimus;

        public BookWorkTask Task { get; set; }

        public BookWorkTaskItem TaskItem { get; set; }

        public IList<CheckTiMuModel> TiMuModels { get; set; }

        /// <summary>
        /// 所有相关目录.
        /// </summary>
        /// <value>
        /// The book chapters.
        /// </value>
        public IList<BookChapter> RelateAllChapters { get; set; }
    }

    /// <summary>
    /// 审核相关信息实体
    /// </summary>
    public class CheckTiMuModel
    {
        /// <summary>
        /// Gets or sets the ti mu model.
        /// </summary>
        /// <value>
        /// The ti mu model.
        /// </value>
        public TiMuModel TiMuModel { get; set; }

        /// <summary>
        /// 大题号
        /// </summary>
        /// <value>
        /// The large number.
        /// </value>
        public string LargeNumber { get; set; }

        /// <summary>
        /// 小题号
        /// </summary>
        /// <value>
        /// The small number.
        /// </value>
        public string SmallNumber { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int? PageNumber { get; set; }

    }
}