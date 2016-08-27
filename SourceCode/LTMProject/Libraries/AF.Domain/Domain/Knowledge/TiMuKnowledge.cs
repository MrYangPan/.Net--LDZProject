using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;
using AF.Domain.Domain.TiMus;

namespace AF.Domain.Domain.Knowledge
{
    public class TiMuKnowledge : BaseEntity
    {
        /// <summary>
        /// 题目id
        /// </summary>
        /// <value>
        /// The tm identifier.
        /// </value>
        public Guid TmId { get; set; }

        /// <summary>
        /// 知识点id
        /// </summary>
        /// <value>
        /// The knowledge identifier.
        /// </value>
        public int KnowledgeId { get; set; }

        /// <summary>
        /// 是否是主知识点
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is main; otherwise, <c>false</c>.
        /// </value>
        public bool IsMain { get; set; }

        /// <summary>
        /// 题目关联表
        /// </summary>
        /// <value>The ti mu.</value>
        public virtual TiMu TiMu { get; set; }

        /// <summary>
        /// 关联的知识点
        /// </summary>
        /// <value>
        /// The knowledge piont.
        /// </value>
        public virtual KnowledgePiont KnowledgePiont { get; set; }
    }
}
