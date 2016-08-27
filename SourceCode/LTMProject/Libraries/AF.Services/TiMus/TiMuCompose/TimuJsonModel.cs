using System;
using System.Collections.Generic;
using System.Web.ModelBinding;
using AF.Domain.Domain.TiMus;

namespace AF.Services.TiMus.TiMuCompose
{
    /// <summary>
    /// 题目json对象
    /// </summary>
    public class TimuJsonModel
    {
        /// <summary>
        /// 题目id
        /// </summary>
        /// <value>
        /// The ti mu identifier.
        /// </value>
        public Guid TiMuId { get; set; }

        /// <summary>
        /// BookTiMu排序
        /// </summary>
        /// <value>
        /// The timu book order.
        /// </value>
        public int? TimuBookOrder { get; set; }

        /// <summary>
        /// 题干
        /// </summary>
        /// <value>
        /// The ti gan.
        /// </value>
        public string TiGan { get; set; }

        /// <summary>
        /// 分析
        /// </summary>
        /// <value>
        /// The analyse.
        /// </value>
        public string Analyse { get; set; }

        /// <summary>
        /// 解答
        /// </summary>
        /// <value>
        /// The explain.
        /// </value>
        public string Explain { get; set; }

        /// <summary>
        /// 点评
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// 题目类别
        /// </summary>
        /// <value>
        /// The type of the choice.
        /// </value>
        public int? ChoiceType { get; set; }

        /// <summary>
        /// 题型
        /// </summary>
        /// <value>
        /// The type of the timu.
        /// </value>
        public int TimuType { get; set; }

        /// <summary>
        /// 正确答案、分值、所有选项集合
        /// </summary>
        /// <value>
        /// The choices iput.
        /// </value>
        public TimuChoice ChoicesIput { get; set; }

        public int SubjectId { get; set; }

        /// <summary>
        /// 大题号
        /// </summary>
        /// <value>
        /// The large number.
        /// </value>
        public string LargeNumber { get; set; }

        /// <summary>
        /// 小题号
        /// </summary>
        /// <value>
        /// The small number.
        /// </value>
        public string SmallNumber { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int? PageNumber { get; set; }
    }

    /// <summary>
    /// 正确答案、分值、所有选项集合
    /// </summary>
    public class TimuChoice
    {
        /// <summary>
        /// 正确答案
        /// </summary>
        /// <value>
        /// The right answer.
        /// </value>
        public string RightAnswer { get; set; }

        /// <summary>
        /// 正确答案标准分
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public int? Score { get; set; }

        /// <summary>
        /// 排列（四行一列，两行两列，一行三列，一行四列）
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public int? Sequence { get; set; }

        /// <summary>
        /// 选项集合
        /// </summary>
        /// <value>
        /// The input choice.
        /// </value>
        public IList<Choices> InputChoice { get; set; }
    }

    /// <summary>
    /// 选项
    /// </summary>
    public class Choices
    {
        /// <summary>
        /// 选项内容
        /// </summary>
        /// <value>
        /// The content of the choice.
        /// </value>
        public string ChoiceContent { get; set; }

        /// <summary>
        /// 选项顺序
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }
    }

    #region 竖式相关序列化类
    /// <summary>
    /// 竖式序列化
    /// </summary>
    public class ShushiTrackSeriaJ
    {
        public string Score { get; set; }
        public IList<TrackChild> Track { get; set; }
        public IList<InputIListChild> InputList { get; set; }
        public HtmlChild Html { get; set; }
    }
    public class TrackChildJ
    {
        public Guid Guid { get; set; }
        public string Operation { get; set; }
    }
    public class InputIListChildJ
    {
        public string Index { get; set; }
        public string Answer { get; set; }
    }
    public class HtmlChildJ
    {
        public Guid G { get; set; }
        public string M { get; set; }
        public IList<TrIListChild> TrList { get; set; }
    }
    public class TrIListChildJ
    {
        public IList<TdListChild> TdList { get; set; }
        public string C { get; set; }
    }
    public class TdListChildJ
    {
        public string C { get; set; }
        public IList<DfnListChild> DfnList { get; set; }
    }
    public class DfnListChildJ
    {
        public Guid G { get; set; }
        public string C { get; set; }
        public string Text { get; set; }
        public int IsAnswer { get; set; }
        public int IsDelete { get; set; }
    }
    #endregion

    /// <summary>
    /// 题目类型（单选、多选、判断、填空）
    /// </summary>
    public enum ChoiceType
    {
        /// <summary>
        /// 单选类
        /// </summary>
        RdoRdo,
        /// <summary>
        /// 多选类
        /// </summary>
        RdoCk,
        /// <summary>
        /// 判断类
        /// </summary>
        RdoTf,
        /// <summary>
        /// 填空类
        /// </summary>
        RdoText,
    }

    /// <summary>
    /// 题目排列（四行一列，两行两列，一行三列，一行四列）
    /// </summary>
    public enum Sequence
    {
        /// <summary>
        /// 四行一列
        /// </summary>
        One,
        /// <summary>
        /// 两行两列
        /// </summary>
        Tow,
        /// <summary>
        /// 一行三列
        /// </summary>
        Three,
        /// <summary>
        /// 一行四列
        /// </summary>
        Four
    }

    /// <summary>
    /// 解析后的题目答案、选项
    /// </summary>
    public class AnswerResult
    {
        public TiMu TiMu { get; set; }

        public List<Input> InputList { get; set; }

        public List<InputChoice> InputChoiceList { get; set; }

        public List<Input> DeleteInputs { get; set; }
    }


    #region 被撤回的任务属性json对象
    public class ReverteJson
    {
        public Guid Tmid { get; set; }
        public int? MainId { get; set; }

        public string MinorIds { get; set; }

        public int Diff { get; set; }

        public string VideoCode { get; set; }

        public string Teacher { get; set; }
    }

    /// <summary>
    /// 用于复合筛选下拉框的数据源，知识点
    /// </summary>
    public class KnowledgeSelectList
    {
        public int Id { get; set; }
        public string KnowName { get; set; }
    }

    #endregion
}