using System.Collections.Generic;
using AF.Core;
using AF.Domain.Domain.BookWork;
using AF.Domain.Domain.Media;

namespace AF.Domain.Domain.Books
{
    /// <summary>
    /// 章节表
    /// </summary>
    public class BookChapter : BaseEntity
    {
        /// <summary>
        /// 章节名称
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        /// <value>
        /// The is lock.
        /// </value>
        public bool IsLock { get; set; }

        /// <summary>
        /// 父章节id
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public int? ParentId { get; set; }

        /// <summary>
        /// 上传的word文件id
        /// </summary>
        /// <value>
        /// The word file identifier.
        /// </value>
        public int WordFileId { get; set; }


        /// <summary>
        /// Gets or sets the task item identifier.
        /// </summary>
        /// <value>
        /// The task item identifier.
        /// </value>
        public int? TaskItemId { get; set; }

        /// <summary>
        /// 图书id
        /// </summary>
        /// <value>
        /// The book identifier.
        /// </value>
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the book chapter parent.
        /// </summary>
        /// <value>
        /// The book chapter parent.
        /// </value>
        public virtual BookChapter BookChapterParent { get; set; }


        /// <summary>
        /// Gets or sets the book work task item.
        /// </summary>
        /// <value>
        /// The book work task item.
        /// </value>
        public virtual BookWorkTaskItem BookWorkTaskItem { get; set; }

        /// <summary>
        /// Gets or sets the upload file.
        /// </summary>
        /// <value>
        /// The upload file.
        /// </value>
        public virtual File UploadFile { get; set; } 

        private ICollection<BookChapter> _bookChapter;

        /// <summary>
        /// Gets or sets the book chapter child.
        /// </summary>
        /// <value>
        /// The book chapter child.
        /// </value>
        public virtual ICollection<BookChapter> BookChapterChild
        {
            get { return _bookChapter ?? (_bookChapter = new List<BookChapter>()); }
            protected set { _bookChapter = value; }
        }

    }
}
