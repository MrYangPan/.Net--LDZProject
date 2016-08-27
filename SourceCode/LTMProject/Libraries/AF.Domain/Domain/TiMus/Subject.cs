using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;
using AF.Domain.Domain.Knowledge;

namespace AF.Domain.Domain.TiMus
{
    /// <summary>
    /// 学科表
    /// </summary>
    public class Subject : BaseEntity,IKvEntity
    {
        /// <summary>
        /// 科目名
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 科目编号
        /// </summary>
        /// <value>
        /// The system code.
        /// </value>
        public string SystemCode { get; set; }

        /// <summary>
        /// 学段
        /// </summary>
        /// <value>
        /// The degree identifier.
        /// </value>
        public int? DegreeId { get; set; }

        /// <summary>
        /// 父级科目id
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public int? ParentId { get; set; }

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

        private ICollection<TiMuType> _tiMuType;
        /// <summary>
        /// 学科关联的题型
        /// </summary>
        public virtual ICollection<TiMuType> TiMuTypes
        {
            get { return _tiMuType ?? (_tiMuType = new List<TiMuType>()); }
            protected set { _tiMuType = value; }
        }

        private ICollection<KnowledgePiont> _knowledge;
        /// <summary>
        /// 学科关联的知识点集合
        /// </summary>
        public virtual ICollection<KnowledgePiont> Knowledges
        {
            get { return _knowledge ?? (_knowledge = new List<KnowledgePiont>()); }
            protected set { _knowledge = value; }
        }

        public string Key => Name;
        public string Value => Id.ToString();
    }
}
