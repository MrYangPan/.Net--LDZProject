using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;

namespace AF.Domain.Domain.Books
{
    /// <summary>
    /// 年级表
    /// </summary>
    public class Grade : BaseEntity, IKvEntity
    {
        /// <summary>
        /// 年级
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 学段
        /// </summary>
        /// <value>
        /// The degreed identifier.
        /// </value>
        public int? DegreeId { get; set; }

        /// <summary>
        ///排序
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int? Order { get; set; }

        public string Key => Name;
        public string Value => Id.ToString();
    }
}
