using System;
using System.Collections.Generic;
using AF.Core;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.Customers;

namespace AF.Domain.Domain.BookWork
{
    public class BookWorkTask : BaseEntity
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 图书id
        /// </summary>
        /// <value>
        /// The book identifier.
        /// </value>
        public int BookId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value>
        /// The create time.
        /// </value>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 录入人
        /// </summary>
        /// <value>
        /// The entry customer identifier.
        /// </value>
        public int? EntryCustomerId { get; set; }

        /// <summary>
        /// 标定人
        /// </summary>
        /// <value>
        /// The mark customer identifier.
        /// </value>
        public int? MarkCustomerId { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        /// <value>
        /// The check customer identifier.
        /// </value>
        public int? CheckCustomerId { get; set; }

        /// <summary>
        /// 任务包含的所有题目都标定无错误.
        /// </summary>
        /// <value>
        /// All tm isvalid.
        /// </value>
        public int? AllTmIsvalid { get; set; }

        /// <summary>
        /// 任务关联的书对象
        /// </summary>
        /// <value>
        /// The book.
        /// </value>
        public virtual Book Book { get; set; }

        /// <summary>
        /// 节点列表
        /// </summary>
        /// <value>
        /// The knowledges.
        /// </value>
        public virtual ICollection<BookWorkTaskItem> BookWorkTaskItems { get; set; }
        
        /// <summary>
        /// 录入人信息
        /// </summary>
        /// <value>
        /// The entry customer.
        /// </value>
        public virtual Customer EntryCustomer { get; set; }

        /// <summary>
        /// 标定人信息
        /// </summary>
        /// <value>
        /// The mark customer.
        /// </value>
        public virtual Customer MarkCustomer { get; set; }

        /// <summary>
        /// 审核人信息
        /// </summary>
        /// <value>
        /// The check customer.
        /// </value>
        public virtual Customer CheckCustomer { get; set; }
    }

    public enum TaskStatus
    {
        Entry = 1,
        Mark = 2,
        Check = 3,
        Revert = 4,
        Complete=5
    }
}
