using System;
using AF.Core;
using AF.Domain.Domain.BookWork;
using AF.Domain.Domain.TiMus;

namespace AF.Domain.Domain.Books
{
    /// <summary>
    /// 章节题目表
    /// </summary>
    public class BookTiMu : BaseEntity
    {
        /// <summary>
        /// 题目id
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public new Guid Id { get; set; }

        /// <summary>
        /// 任务子项id
        /// </summary>
        /// <value>
        /// The task item identifier.
        /// </value>
        public int TaskItemId { get; set; }

        /// <summary>
        /// 题目审核状态(该题是否有错).
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public TimuStatus Status { get; set; }

        /// <summary>
        /// 图书id
        /// </summary>
        /// <value>
        /// The book identifier.
        /// </value>
        public int BookId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

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

        /// <summary>
        /// 标记错误注释
        /// </summary>
        /// <value>
        /// The error information.
        /// </value>
        public string ErrorInfo { get; set; }

        /// <summary>
        /// 题目.
        /// </summary>
        /// <value>
        /// The timu.
        /// </value>
        public virtual TiMu Timu { get; set; }

        /// <summary>
        /// 任务子项.
        /// </summary>
        /// <value>
        /// The task item.
        /// </value>
        public virtual BookWorkTaskItem TaskItem { get; set; }

        public enum TimuStatus
        {
            Invalid = 0,
            Valid = 1
        }
    }
}
