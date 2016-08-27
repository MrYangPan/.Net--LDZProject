using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;

namespace AF.Domain.Domain.Books
{
    /// <summary>
    /// 学段表
    /// </summary>
    public class Degree : BaseEntity,IKvEntity
    {
        /// <summary>
        /// 学段名
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int? Order { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; set; }

        public string Key => Name;
        public string Value => Id.ToString();
    }
}
