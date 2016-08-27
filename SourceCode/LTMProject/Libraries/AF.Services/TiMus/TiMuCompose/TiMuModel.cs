using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.TiMus;

namespace AF.Services.TiMus.TiMuCompose
{
    /// <summary>
    /// 解析后的题目信息，用于页面预览展示
    /// </summary>
    public class TiMuModel
    {
        /// <summary>
        /// 题目id
        /// </summary>
        /// <value>
        /// The ti mu identifier.
        /// </value>
        public Guid Tmid { get; set; }

        /// <summary>
        /// 题干
        /// </summary>
        /// <value>
        /// The trunk.
        /// </value>
        public string Trunk { get; set; }

        /// <summary>
        /// 难度
        /// </summary>
        /// <value>
        /// The difficult.
        /// </value>
        public int Difficult { get; set; }

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
        /// 主知识点名称
        /// </summary>
        /// <value>
        /// The knowledge identifier.
        /// </value>
        public string MainKnowledgeId { get; set; }

        /// <summary>
        /// 相关知识点名称集合
        /// </summary>
        /// <value>
        /// The minor knowledge identifier.
        /// </value>
        public string MinorKnowledgeId { get; set; }

        /// <summary>
        /// 题型
        /// </summary>
        /// <value>
        /// The name of the ti mu type.
        /// </value>
        public string TiMuTypeName { get; set; }

        /// <summary>
        /// 讲解老师
        /// </summary>
        /// <value>
        /// The explain teacher.
        /// </value>
        public string ExplainTeacher { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public BookTiMu.TimuStatus Status { get; set; }

        /// <summary>
        /// 标记错误信息
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the video code.
        /// </summary>
        /// <value>
        /// The video code.
        /// </value>
        public string VideoCode { get; set; }

        /// <summary>
        /// 排序 (对应BookTiMu表的order)
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// 子题目集合
        /// </summary>
        /// <value>
        /// The child timu list.
        /// </value>
        public IList<TiMuModel> ChildTimuList { get; set; }

    }
}
