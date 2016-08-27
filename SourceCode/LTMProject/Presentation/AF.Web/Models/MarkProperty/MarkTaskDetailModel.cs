using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;
using AF.Web.Framework;

namespace AF.Web.Models.MarkProperty
{
    public class MarkTaskDetailModel:BaseEntityModel
    {
        public MarkTaskDetailModel()
        {
            TaskRelatedChapter=new List<BookChapter>();
            RelateChapters = new List<BookChapter>();
            RelateAllChapters = new List<BookChapter>();
        }


        /// <summary>
        /// 任务包含的所有目录
        /// </summary>
        public IList<BookChapter> TaskRelatedChapter;

        /// <summary>
        /// 任务相关的树形一级目录
        /// </summary>
        public IList<BookChapter> RelateChapters;

        /// <summary>
        /// 任务相关的树形所有目录
        /// </summary>
        public IList<BookChapter> RelateAllChapters;

        public int TaskId { get; set; }
    }
}