using System;
using System.Collections.Generic;
using AF.Core;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.Knowledge;

namespace AF.Domain.Domain.TiMus
{
    /// <summary>
    /// 题目表
    /// </summary>
    public class TiMu : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public new Guid Id { get; set; }

        /// <summary>
        /// 学科
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public int SubjectId { get; set; }

        /// <summary>
        /// 题型
        /// </summary>
        /// <value>
        /// The ti mu type identifier.
        /// </value>
        public int TiMuTypeId { get; set; }

        /// <summary>
        /// 题干
        /// </summary>
        /// <value>
        /// The trunk.
        /// </value>
        public string Trunk { get; set; }

        /// <summary>
        /// 分析
        /// </summary>
        /// <value>
        /// The analysis.
        /// </value>
        public string Analysis { get; set; }

        /// <summary>
        /// 解答
        /// </summary>
        /// <value>
        /// The answer.
        /// </value>
        public string Answer { get; set; }

        /// <summary>
        /// 点评
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// 难度
        /// </summary>
        /// <value>
        /// The difficult.
        /// </value>
        public int Difficult { get; set; }

        /// <summary>
        /// 区分度
        /// </summary>
        /// <value>
        /// The discriminate.
        /// </value>
        public int? Discriminate { get; set; }

        /// <summary>
        /// 标准用时
        /// </summary>
        /// <value>
        /// The standard time.
        /// </value>
        public int? StandardTime { get; set; }

        /// <summary>
        /// 标准分值
        /// </summary>
        /// <value>
        /// The standerd scroe.
        /// </value>
        public int? StanderdScroe { get; set; }

        /// <summary>
        /// 能力类型id
        /// </summary>
        /// <value>
        /// The ability identifier.
        /// </value>
        public int AbilityId { get; set; }

        /// <summary>
        /// 年级
        /// </summary>
        /// <value>
        /// The grade identifier.
        /// </value>
        public int? GradeId { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int? Year { get; set; }

        /// <summary>
        /// 试题来源
        /// </summary>
        /// <value>
        /// The soure.
        /// </value>
        public string Soure { get; set; }

        /// <summary>
        /// 质量评价id
        /// </summary>
        /// <value>
        /// The quality identifier.
        /// </value>
        public int? QualityId { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; set; }

        /// <summary>
        /// 题目关联的题目扩展类对象
        /// </summary>
        /// <value>
        /// The ti mu attribute extend.
        /// </value>
        public virtual TiMuAttributeExtend TiMuAttributeExtend { get; set; }

        /// <summary>
        /// 题目关联的学科对象
        /// </summary>
        public virtual Subject Subject { get; set; }

        /// <summary>
        /// 题目关联的题目类型对象
        /// </summary>
        /// <value>
        /// The type of the ti mu.
        /// </value>
        public virtual TiMuType TiMuType { get; set; }

        /// <summary>
        /// 题目关联的能力类型对象
        /// </summary>
        /// <value>
        /// The ability.
        /// </value>
        public virtual Ability Ability { get; set; }

        /// <summary>
        /// 题目关联的年级对象
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public virtual Grade Grade { get; set; }

        /// <summary>
        /// 视频编号
        /// </summary>
        /// <value>
        /// The video code.
        /// </value>
        public string VideoCode { get; set; }

        private ICollection<Input> _input;
        /// <summary>
        /// 题目关联的答案集合
        /// </summary>
        public virtual ICollection<Input> Inputs
        {
            get { return _input ?? (_input = new List<Input>()); }
            protected set { _input = value; }
        }

        private ICollection<TiMuKnowledge> _knowledge;
        /// <summary>
        /// 题目关联的题目知识点关系表集合
        /// </summary>
        public virtual ICollection<TiMuKnowledge> Knowledges
        {
            get { return _knowledge ?? (_knowledge = new List<TiMuKnowledge>()); }
            protected set { _knowledge = value; }
        }

        /// <summary>
        /// 题目关联的BookTiMu对象
        /// </summary>
        /// <value>
        /// The book ti mu.
        /// </value>
        public virtual BookTiMu BookTiMu { get; set; }

        /// <summary>
        /// 父题目id
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public virtual Guid? ParentId{ get; set; }

        /// <summary>
        /// 父题目
        /// </summary>
        /// <value>
        /// The ti mu parent.
        /// </value>
        public virtual TiMu TiMuParent { get; set; }

        private ICollection<TiMu> _tiMuChapter;

        /// <summary>
        /// 子题目集合
        /// </summary>
        /// <value>
        /// The book chapter child.
        /// </value>
        public virtual ICollection<TiMu> TiMuChild
        {
            get { return _tiMuChapter ?? (_tiMuChapter = new List<TiMu>()); }
            protected set { _tiMuChapter = value; }
        }

    }
}
