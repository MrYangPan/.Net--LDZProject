using System;
using System.Collections.Generic;

namespace AF.Services.TiMus.TiMuCompose
{
    /// <summary>
    /// 答案类型为online-input的序列化类
    /// </summary>
    public class TmInputAnswerOnlineInput
    {
        public IList<string> Answer { get; set; }
        public IList<string> Attr { get; set; }
        public IList<string> Left { get; set; }
        public IList<string> Right { get; set; }
    }

    /// <summary>
    /// 答案类型为duoguo-input的序列化类
    /// </summary>
    public class TmInputAnswerDuoguoInput
    {
        public string Answer { get; set; }
        public IList<string> Attr { get; set; }
    }

    #region 竖式相关序列化类
    /// <summary>
    /// 竖式序列化
    /// </summary>
    public class ShushiTrackSeria
    {
        public string Score { get; set; }
        public IList<TrackChild> Track { get; set; }
        public IList<InputIListChild> InputList { get; set; }
        public HtmlChild Html { get; set; }
    }
    public class TrackChild
    {
        public Guid guid { get; set; }
        public string Operation { get; set; }
    }
    public class InputIListChild
    {
        public string Index { get; set; }
        public string Answer { get; set; }
    }
    public class HtmlChild
    {
        public Guid G { get; set; }
        public string M { get; set; }
        public IList<TrIListChild> TrList { get; set; }
    }
    public class TrIListChild
    {
        public IList<TdListChild> TdList { get; set; }
        public string C { get; set; }
    }
    public class TdListChild
    {
        public string C { get; set; }
        public IList<DfnListChild> DfnList { get; set; }
    }
    public class DfnListChild
    {
        public Guid G { get; set; }
        public string C { get; set; }
        public string Text { get; set; }
        public int IsAnswer { get; set; }
        public int IsDelete { get; set; }
    }

    /// <summary>
    /// 保存竖式答案相关json
    /// </summary>
    public class GroupKongAnswer
    {
        public string Attr { get; set; }

        public Guid Guid { get; set; }

        public List<GKong> GKong { get; set; }
    }

    public class GKong
    {
        public int Order { get; set; }
        public Dictionary<string, AnswerList> AnswerList { get; set; }
    }

    public class AnswerList
    {
        public string Answer { get; set; }

        public decimal Score { get; set; }
    }

    #endregion

    public class ShuShiCorrectAnswer
    {
        public string[] Attr { get; set; }

        public string Guid { get; set; }

        public IList<IDictionary<string, object>> AnswerList { get; set; }
    }

    public class CorrectAnswer
    {
        public string answer { get; set; }
        public string score { get; set; }
    }

}
