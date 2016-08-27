using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Domain.Domain.Books;
using AF.Web.Framework;

namespace AF.Web.Models.CheckPublish
{
    public class CheckTaskDetailModel : BaseEntityModel
    {
        public CheckTaskDetailModel()
        {
            TaskRelatedChapter = new List<BookChapter>();
            RelateChapters = new List<BookChapter>();
        }

        /// <summary>
        /// 任务相关的所有目录ID
        /// </summary>
        public IList<BookChapter> TaskRelatedChapter;

        /// <summary>
        /// 任务相关的树形一级目录
        /// </summary>
        public IList<BookChapter> RelateChapters;

        public int TaskId { get; set; }
    }
}