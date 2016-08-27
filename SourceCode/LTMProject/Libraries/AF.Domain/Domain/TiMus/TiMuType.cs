using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;

namespace AF.Domain.Domain.TiMus
{
    /// <summary>
    /// 题目类型表
    /// </summary>
    public class TiMuType : BaseEntity
    {
        /// <summary>
        /// 学科id
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public int SubjectId { get; set; }

        /// <summary>
        /// 题型编码
        /// </summary>
        /// <value>
        /// The name of the system.
        /// </value>
        public string SystemName { get; set; }

        /// <summary>
        /// 题型名称
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <value>
        /// The ordering.
        /// </value>
        public int Ordering { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
