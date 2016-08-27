using System;
using AF.Core;

namespace AF.Domain.Domain.TiMus
{
    /// <summary>
    /// 题目扩展表
    /// </summary>
    public class TiMuAttributeExtend : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public new Guid Id { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        /// <value>
        /// The sear key word.
        /// </value>
        public string SearKeyWord { get; set; }

        /// <summary>
        /// 语种id
        /// </summary>
        /// <value>
        /// The language identifier.
        /// </value>
        public int? LanguageId { get; set; }

        /// <summary>
        /// 生存期
        /// </summary>
        /// <value>
        /// The limit time.
        /// </value>
        public string LimitTime { get; set; }

        /// <summary>
        /// 教材版本id
        /// </summary>
        /// <value>
        /// The publish identifier.
        /// </value>
        public int? PublishId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value>
        /// 中考标记
        /// </value>
        public string MidTestMark { get; set; }

        /// <summary>
        /// 组卷次数
        /// </summary>
        /// <value>
        /// The combine count.
        /// </value>
        public int? CombineCount { get; set; }

        /// <summary>
        /// 做题次数
        /// </summary>
        /// <value>
        /// The do count.
        /// </value>
        public int? DoCount { get; set; }

        /// <summary>
        /// 实测难度
        /// </summary>
        /// <value>
        /// The real difference.
        /// </value>
        public int? RealDiff { get; set; }

        /// <summary>
        /// 实测区分度
        /// </summary>
        /// <value>
        /// The real discriminate.
        /// </value>
        public int? RealDiscriminate { get; set; }

        /// <summary>
        /// 出题人
        /// </summary>
        /// <value>
        /// The examinator identifier.
        /// </value>
        public int? ExaminatorId { get; set; }

        /// <summary>
        /// 题目
        /// </summary>
        /// <value>
        /// The ti mu.
        /// </value>
        public virtual TiMu TiMu { get; set; }

    }
}
