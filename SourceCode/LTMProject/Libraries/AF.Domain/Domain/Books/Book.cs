using System;
using System.Security.Policy;
using AF.Core;
using AF.Domain.Domain.TiMus;

namespace AF.Domain.Domain.Books
{
    public class Book : BaseEntity
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
        public int DegreeId { get; set; }

        /// <summary>
        /// 年级id
        /// </summary>
        /// <value>
        /// The grade identifier.
        /// </value>
        public int GradeId { get; set; }

        /// <summary>
        /// Gets or sets the term identifier.
        /// </summary>
        /// <value>
        /// The term identifier.
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
        /// 创建时间
        /// </summary>
        /// <value>
        /// The create time.
        /// </value>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// 书关联的学科对象
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public virtual Subject Subject { get; set; }

        /// <summary>
        /// 书关联的学段对象
        /// </summary>
        /// <value>
        /// The degree.
        /// </value>
        public virtual Degree Degree { get; set; }

        /// <summary>
        /// 书关联的年级对象
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public virtual Grade Grade { get; set; }

        /// <summary>
        /// Gets or sets the publisher.
        /// </summary>
        /// <value>
        /// The publisher.
        /// </value>
        public virtual Publisher Publisher { get; set; }


        /// <summary>
        /// 学期
        /// </summary>
        /// <value>
        /// The term.
        /// </value>
        public virtual Term Term { get; set; } 
    }



}
