using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;

namespace AF.Domain.Domain.TiMus
{
    /// <summary>
    /// 答案选项
    /// </summary>
    public class InputChoice : BaseEntity
    {
        public new Guid Id { get; set; }

        /// <summary>
        /// 答案id
        /// </summary>
        /// <value>
        /// The input identifier.
        /// </value>
        public Guid InputId { get; set; }

        /// <summary>
        /// 选项内容
        /// </summary>
        /// <value>
        /// The content of the choice.
        /// </value>
        public string ChoiceContent { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public decimal Score { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// 选项关联的答案对象
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public virtual Input Input { get; set; }

    }
}
