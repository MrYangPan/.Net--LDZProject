using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Services.Books
{
    public class BookRequest
    {
        /// <summary>
        /// 学科id
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public int SubjectId { get; set; }

        /// <summary>
        /// 学段id
        /// </summary>
        /// <value>
        /// The degree identifier.
        /// </value>
        public int? DegreeId { get; set; }

        /// <summary>
        /// 年级id
        /// </summary>
        /// <value>
        /// The grade identifier.
        /// </value>
        public int GradeId { get; set; }

        /// <summary>
        /// 学期
        /// </summary>
        /// <value>
        /// The term.
        /// </value>
        public int TermId { get; set; }

        /// <summary>
        /// 教辅年份
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year { get; set; }

        /// <summary>
        /// ISBN编号
        /// </summary>
        /// <value>
        /// The isbn.
        /// </value>
        public string Isbn { get; set; }

        /// <summary>
        /// 书名
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 出版社
        /// </summary>
        /// <value>
        /// The publisher.
        /// </value>
        public int PublisherId { get; set; }

        /// <summary>
        /// 搜索开始时间
        /// </summary>
        /// <value>
        /// The create time.
        /// </value>
        public DateTime? Begin { get; set; }

        /// <summary>
        /// 搜索结束时间
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public DateTime? End { get; set; }

        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int PageIndex { get; set; } = 0;
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; } = int.MaxValue;

    }
}
