using System.Collections.Generic;
using AF.Core;
using AF.Domain.Domain.Books;

namespace AF.Domain.Domain.BookWork
{
    /// <summary>
    /// 图书任务子项
    /// </summary>
    public class BookWorkTaskItem : BaseEntity
    {
        /// <summary>
        /// 任务id
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public int TaskId { get; set; }

        /// <summary>
        /// 章节id
        /// </summary>
        /// <value>
        /// The chapter identifier.
        /// </value>
        public int ChapterId { get; set; }

        /// <summary>
        /// 状态(判断本章节是否有错题) 0：没有  1：有
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }

        /// <summary>
        /// 任务信息
        /// </summary>
        /// <value>
        /// The book work task.
        /// </value>
        public virtual BookWorkTask BookWorkTask { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        /// <value>
        /// The book chapter.
        /// </value>
        public virtual BookChapter BookChapter { get; set; }

        /// <summary>
        /// 题目表信息
        /// </summary>
        /// <value>
        /// The task book ti mus.
        /// </value>
        public virtual ICollection<BookTiMu> TaskBookTiMus { get; set; }
    }
}
