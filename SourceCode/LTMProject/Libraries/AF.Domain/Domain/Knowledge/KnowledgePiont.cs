using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;
using AF.Domain.Domain.TiMus;

namespace AF.Domain.Domain.Knowledge
{
    public class KnowledgePiont : BaseEntity
    {
        /// <summary>
        /// 学科id
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public int SubjectId { get; set; }

        /// <summary>
        /// 知识点名称
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 父知识点id
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public int? ParentId { get; set; }

        /// <summary>
        /// 知识点关联的学科
        /// </summary>
        /// <value>
        /// The suject.
        /// </value>
        public virtual Subject Suject { get; set; }

        private ICollection<TiMuKnowledge> _tiMuKnowledge;
        /// <summary>
        /// 关联的题目知识点
        /// </summary>
        public virtual ICollection<TiMuKnowledge> TiMuKnowledge
        {
            get { return _tiMuKnowledge ?? (_tiMuKnowledge = new List<TiMuKnowledge>()); }
            protected set { _tiMuKnowledge = value; }
        }
    }
}
