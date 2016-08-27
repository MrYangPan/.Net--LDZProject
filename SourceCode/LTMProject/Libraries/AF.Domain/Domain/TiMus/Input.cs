using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;

namespace AF.Domain.Domain.TiMus
{
    /// <summary>
    /// 题目答案
    /// </summary>
    public class Input : BaseEntity
    {
        public new Guid Id { get; set; }

        /// <summary>
        /// 题目id
        /// </summary>
        /// <value>
        /// The tm identifier.
        /// </value>
        public Guid TmId { get; set; }

        /// <summary>
        /// 答案code
        /// </summary>
        /// <value>
        /// The input code.
        /// </value>
        public string InputCode { get; set; }

        /// <summary>
        /// 基础类型
        /// </summary>
        /// <value>
        /// The type of the base.
        /// </value>
        public string BaseType { get; set; }

        /// <summary>
        /// 答案类型
        /// </summary>
        /// <value>
        /// The type of the input.
        /// </value>
        public string InputType { get; set; }

        /// <summary>
        /// 答案内容
        /// </summary>
        /// <value>
        /// The input answer.
        /// </value>
        public string InputAnswer { get; set; }

        /// <summary>
        /// 标准分
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public decimal Score { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        /// <value>
        /// The input score.
        /// </value>
        public string InputScore { get; set; }

        /// <summary>
        /// 答案关联的题目对象
        /// </summary>
        /// <value>
        /// The ti mu.
        /// </value>
        public virtual TiMu TiMu { get; set; }

        private ICollection<InputChoice> _inputChoice;
        /// <summary>
        /// 答案关联的选项集合
        /// </summary>
        public virtual ICollection<InputChoice> InputChoice
        {
            get { return _inputChoice ?? (_inputChoice = new List<InputChoice>()); }
            protected set { _inputChoice = value; }
        }
    }
}
