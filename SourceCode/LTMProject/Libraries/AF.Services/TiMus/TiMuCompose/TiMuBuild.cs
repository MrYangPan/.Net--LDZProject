using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using AF.Core.Configuration;
using AF.Core.Extensions;
using AF.Core.Infrastructure;
using AF.Domain.Domain.Knowledge;
using AF.Domain.Domain.TiMus;
using Newtonsoft.Json;

namespace AF.Services.TiMus.TiMuCompose
{
    /// <summary>
    /// 拼题目和题解(判分 反思 拓展练习 微课...)  yp 2015-12-29
    /// </summary>
    public class TiMuBuild
    {
        #region Fields

        private string _srcPriorIm = "/Content/Upload/"; //题目常规图片src地址
        private readonly string _mathmlImg ; //数学公式地址
        private readonly TiMuBuiltRequest _muBuiltRequest;
        private readonly TiMu _tiMu;

        #endregion

        #region .ctor

        public TiMuBuild(TiMuBuiltRequest request)
        {
            _mathmlImg = Singleton<AFConfig>.Instance.MathHost;
            _muBuiltRequest = request;
            _tiMu = request.TiMu;
        }

        #endregion

        #region 拼题

        public string BuildHtml()
        {
            string oldtimu = _tiMu.Trunk;
            if (string.IsNullOrWhiteSpace(oldtimu))
            {
                return String.Empty;
            }
            string tiMuUpper = _tiMu.Id.ToString();
            string tmid = _tiMu.Id.ToString();
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append(@"<div class=""Examination"">
                                    <div class=""examination_attribute"" id=""test_$TmidToUpper$""></div>
                                    <div class=""marks"" id=""mark_$TmidToUpper$""  style=""display: none;"">
                                        <table cellspacing=""0"" cellpadding=""0"">
                                           <tbody>
                                                <tr>
                                                <td class=""zhongkao"" id=""zhongkao_$TmidToUpper$"" ></td>
                                                <td class=""nandu"" id=""nandu_$TmidToUpper$""></td>
                                                </tr>
                                           <tbody>
                                        </table>
                                     </div>     
                                     <div class=""h1"" id=""item_$TmidToUpper$"">
                                        <div class=""topicbody"">
                                            <dfn class=""Serialnumber"">$Index$</dfn>
                                             $TiMuHTML$
                                        </div>
                                        <div class=""h1_Analytic"" id=""answerall_$TmidToUpper$"" [$style]>[$Analytic$]</div>
                                     </div>
                                </div>");
            oldtimu = ReplaceShuShi(oldtimu, null);
            if (_tiMu.Inputs.Count != 0)
            {
                var inputList = _tiMu.Inputs.OrderBy(d => d.Order).ToList();
                oldtimu = RegexContent(FormatKongStyle(oldtimu, inputList));
                //正则替换kbd
                Regex regexkbd = new Regex("<kbd>([\\s\\S]*?)</kbd>");
                //判断题目答案类型 题目类型
                if (inputList.Count > 0)
                {
                    for (int i = 0; i < inputList.Count; i++)
                    {
                        var input = inputList.ToList()[i];
                        if (inputList[i].BaseType == "choice" && inputList[i].InputType != "radio-TF")
                        {
                            string replaceHtml = PingChoiceHtml(inputList[i]);

                            oldtimu = regexkbd.Replace(oldtimu, replaceHtml);
                        }
                        else
                        {
                            oldtimu = regexkbd.Replace(oldtimu,
                                string.Format(
                                    @"<div class=""FEBox"" id=""i_{0}"" name=""{0}"" type=""otherinput"" editable=""true""></div>",
                                    input.InputCode), 1, 0);
                        }
                    }
                }
            }
            //如果要生成pdf试卷，则移除答案解析标签
            if (_muBuiltRequest.IsCreatePdf)
                sBuilder.Replace(
                    @"<div class=""h1_Analytic"" id=""answerall_$TmidToUpper$"" [$style]>[$Analytic$]</div>", "");
            sBuilder.Replace("test_$TmidToUpper$", "test_" + tiMuUpper)
                .Replace("mark_$TmidToUpper$", "mark_" + tiMuUpper)
                .Replace("zhongkao_$TmidToUpper$", "zhongkao_" + tiMuUpper)
                .Replace("nandu_$TmidToUpper$", "nandu_" + tiMuUpper)
                .Replace("item_$TmidToUpper$", "item_" + tiMuUpper)
                .Replace("$Index$", _muBuiltRequest.Index ?? string.Empty)
                .Replace("answerall_$TmidToUpper$", "answerall_" + tiMuUpper)
                .Replace("$TiMuHTML$", oldtimu)
                .Replace("[$Analytic$]", TiMuAnalyse(tiMuUpper))
                .Replace("[$style]", _muBuiltRequest.IsAnalyticVisable ? "style=display:block;" : "style=display:none;");
            return sBuilder.ToString();
        }

        /// <summary>
        /// 题目图片和公式解析
        /// </summary>
        /// <param name="content"></param>
        /// <param name="tmid"></param>
        /// <returns></returns>
        private string RegexContent(string content)
        {
            if (_muBuiltRequest.ApplicationUrl != null)
            {
                _srcPriorIm = _muBuiltRequest.ApplicationUrl + "/Content/Upload/";
            }
            var subjectCode = _tiMu.Subject.SystemCode;
            //正则替换image图片的三种格式
            Regex regexs = new Regex("<img[^>]+src\\s*=\\s*['\"]([^'\"]+)['\"][^>]*>");
            Regex RegImg = new Regex("\\[img=(\\d+),(\\d+)\\](.*?)\\[/img\\]", RegexOptions.Multiline);
            Regex regImg1 = new Regex("\\[img=(\\d+),(\\d+),(\\d+)\\](.*?)\\[/img\\]", RegexOptions.Multiline);
            Regex regYb = new Regex("\\[yb\\](.*?)\\[\\/yb\\]", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection regImg = regexs.Matches(content);
            foreach (Match m in regImg)
            {
                string MatchStr = m.Groups[1].Value;
                string Src = MatchStr.Substring(MatchStr.LastIndexOf('/') + 1);
                Src = _srcPriorIm + subjectCode.Trim() + "/" + Src;
                string NewMatchStr = Regex.Replace(m.Value, @"(?is)(?<=<img[^>]+src="").+?(?=""[^>]*/>)", Src);
                content = content.Replace(m.Value, NewMatchStr);
            }
            MatchCollection MCImgs = RegImg.Matches(content);
            foreach (Match m in MCImgs)
            {
                string matchStr = m.Groups[0].Value;
                string width = m.Groups[1].Value;
                string height = m.Groups[2].Value;
                string src = m.Groups[3].Value;
                src = src.Substring(src.LastIndexOf('/') + 1);
                src = _srcPriorIm + subjectCode.Trim() + "/" + src;
                string imgTag = "<img src=\"" + src + "\" style=\"vertical-align:middle\" />";
                content = content.Replace(matchStr, imgTag);
            }
            MCImgs = regImg1.Matches(content);
            foreach (Match m in MCImgs)
            {
                string matchStr = m.Groups[0].Value;
                string width = m.Groups[1].Value;
                string height = m.Groups[2].Value;
                int align = int.Parse(m.Groups[3].Value);
                string src = m.Groups[4].Value;
                src = src.Substring(src.LastIndexOf('/') + 1);
                src = _srcPriorIm + subjectCode.Trim() + "/" + src;
                string imgTag = "<img src=\"" + src + "\" style=\"vertical-align:middle\" />";
                if (align == 1)
                {
                    imgTag = "<div align=\"center\">" + imgTag + "</div>";
                }
                else if (align == 2)
                {
                    imgTag = "<div align=\"right\">" + imgTag + "</div>";
                }
                content = content.Replace(matchStr, imgTag);
            }
            MatchCollection mcyb = regYb.Matches(content);
            foreach (Match m in mcyb)
            {
                string matchStr = m.Groups[0].Value;
                string yb = m.Groups[1].Value;
                content = content.Replace(matchStr, "<bdo>[" + yb + "]</bdo>");
            }
            content = content.Replace("\r", "<br />")
                .Replace("<nobr>", "")
                .Replace("</nobr>", "")
                .Replace("<nobr/>", "");
            content = Regex.Replace(content, "\\[align=(\\w{4,6})\\]([\\s\\S]*?)\\[\\/align\\]",
                "<div align=\"$1\">$2</div>", RegexOptions.Multiline);
            content = Regex.Replace(content, "<c>(.*?)</c>", "<div align=\"center\">$1</div>", RegexOptions.Multiline);
            content = Regex.Replace(content, "<r>(.*?)</r>", "<div align=\"right\">$1</div>", RegexOptions.Multiline);
            content = Regex.Replace(content, "\\[font=(.*?)\\]([\\s\\S]*?)\\[/font\\]", "<font face=\"$1\">$2</font>",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "\\[size=(.*?)\\]([\\s\\S]*?)\\[/size\\]",
                "<abbr style=\"font-size:$1px;\">$2</abbr>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "\\[color=(.*?)\\]([\\s\\S]*?)\\[/color\\]", "<font color=\"$1\">$2</font>",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "\\[textstyle=(.*?)\\]([\\s\\S]*?)\\[/textstyle\\]",
                new MatchEvaluator(TextTarget), RegexOptions.Multiline | RegexOptions.IgnoreCase);

            content = MathMlReplace(content);
            return content;
        }


        /// <summary>
        ///公式替换
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private string MathMlReplace(string content)
        {
            //正则表达式替换``中的公式内容
            var regex = new Regex(@"`(.*?)`");
            var listTm = regex.Matches(content);
            foreach (Match item in listTm)
            {
                var gs = item.Value;
                var imageurl = $"{_mathmlImg}/ImageTemp/{GetMd5Hash(gs)}.jpg";
                var errorsrc = $"{_mathmlImg}?m={System.Web.HttpUtility.UrlEncode(gs)}";
                string img =
                    $@"<img class=""mathmlImg""   src='{imageurl}'  onerror=""this.src ='{errorsrc
                        }';this.onerror = null;""  />";
                content = content.Replace(gs, img);
            }
            return content;
        }


        private string TextTarget(Match m)
        {
            string fh = m.Groups[1].Value;
            string val = m.Groups[2].Value;
            if (fh == "dian" || fh == "sanjiao")
            {
                string txtDian = string.Empty;
                for (int i = 0; i < val.Length; i++)
                {
                    txtDian += "<span class=\"" + fh + "\">" + val.Substring(i, 1) + "</span>";
                }
                return txtDian;
            }
            if (fh == "double" || fh == "border")
            {
                return "<span class=\"" + fh + "\">" + val + "</span>";
            }
            return m.Groups[0].Value;
        }

        /// <summary>
        /// 拼接竖式
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="inputList">The input list.</param>
        /// <param name="shuShiCorrectAnswer">The shu shi correct answer.</param>
        /// <returns></returns>
        private string ParserShushi(string guid, List<Input> inputList, IList<ShuShiCorrectAnswer> shuShiCorrectAnswer)
        {
            string shushiJson = GetShuShiTrackData(guid);
            if (shushiJson == string.Empty) return "没有该竖式(" + guid + ")的数据!";
            try
            {
                ShushiTrackSeria shushiTrackSeria = shushiJson.Json2Obj<ShushiTrackSeria>();
                StringBuilder html = new StringBuilder();
                List<string> inputStrList = new List<string>();
                //用于查找用户答案
                int l = 0;
                html.Append(
                    string.Format(
                        @"<table model=""{0}"" id=""{1}"" class=""ShuShi"" cellpadding=""0"" cellspacing=""0""><tbody>",
                        shushiTrackSeria.Html.M.ToString(), guid));
                for (int i = 0; i < shushiTrackSeria.Html.TrList.Count; i++)
                {
                    Input inputAnswer = null;
                    if (inputList != null && inputList.Any())
                        inputAnswer = inputList[i];
                    var trList = shushiTrackSeria.Html.TrList[i];
                    html.Append("<tr ");
                    if (trList.C != null && trList.C.ToString() != "") html.Append("class=\"" + trList.C + "\" ");
                    html.Append('>');
                    for (int j = 0; j < trList.TdList.Count; j++)
                    {
                        var TdList = shushiTrackSeria.Html.TrList[i].TdList[j];
                        html.Append("<td ");
                        if (!string.IsNullOrEmpty(TdList.C)) html.Append("class=\"" + TdList.C + "\" ");
                        html.Append('>');
                        for (int k = 0; k < TdList.DfnList.Count; k++)
                        {
                            var dfnList = TdList.DfnList[k];
                            var isDaankuang = dfnList.C?.Contains("daankuang") ?? false;
                            if (dfnList.IsAnswer.ToString() == "1" && !isDaankuang)
                            {
                                html.Append("<kbd></kbd>");
                            }
                            else
                            {
                                html.Append("<div ");
                                if (dfnList.C != null && dfnList.C.ToString() != "")
                                {
                                    if (isDaankuang && shuShiCorrectAnswer != null)
                                    {
                                        //填充答案后，禁止编辑
                                        html.Append(
                                            string.Format(
                                                @"class={0} contenteditable=false  id={1} name={2} type=""shushi""  no-empty",
                                                "daankuang", inputAnswer.InputCode + "_" + k, inputAnswer.InputCode));
                                        inputStrList.Add(inputAnswer.InputCode + "_" + k);
                                    }
                                    else if (isDaankuang)
                                    {
                                        html.Append(
                                            string.Format(
                                                @"class={0} contenteditable=true  id={1} name={2} type=""shushi""  no-empty  $style",
                                                "daankuang", inputAnswer.InputCode + "_" + k, inputAnswer.InputCode));
                                        inputStrList.Add(inputAnswer.InputCode + "_" + k);
                                    }
                                    else
                                    {
                                        if (dfnList.Text.ToString() == ".")
                                        {
                                            html.Append("class=\"decimalPoint\" ");
                                        }
                                        else
                                        {
                                            html.Append("class=\"" + dfnList.Text + "\" ");
                                        }
                                    }
                                }
                                html.Append(">");
                                //题目的分析解答点评
                                if (inputStrList.Count > 0 && isDaankuang && shuShiCorrectAnswer != null)
                                {
                                    html.Append(string.Format(@"<div style=""display: none;"">{0}</div>",
                                        "[$]" + inputAnswer.InputCode + "_" + k));
                                }
                                if (dfnList.IsAnswer.ToString() == "0" && dfnList.Text.ToString() != "")
                                    html.Append(dfnList.Text.ToString());

                                #region 用户答案填充

                                ////如果是输入框的，才做以下操作
                                //if (isDaankuang)
                                //{
                                //    var nextInputAnswer = inputList[l];
                                //    //用户答案
                                //    if (_muBuiltRequest.IsShowUserAnswer)
                                //    {
                                //        var userAnswer = _muBuiltRequest.UserResult;
                                //        if (userAnswer != null && userAnswer.Any() &&
                                //            (inputList != null && inputList.Any()))
                                //        {
                                //            //根据inputCode判断用户答案是否存在
                                //            if (userAnswer.Select(d => d.InputCode).Contains(nextInputAnswer.InputCode))
                                //            {
                                //                //获取用户输入的答案
                                //                var userA =
                                //                    userAnswer.FirstOrDefault(
                                //                        d => d.InputCode == nextInputAnswer.InputCode);
                                //                //序列化标准答案，用于比较用户输入的答案是否正确
                                //                var correctAnswer =
                                //                    JsonConvert.DeserializeObject<IList<ShuShiCorrectAnswer>>(
                                //                        nextInputAnswer.InputAnswer);
                                //                correctAnswer.ToList().ForEach(p =>
                                //                {
                                //                    p.AnswerList.ToList().ForEach(q =>
                                //                    {
                                //                        var iputanswer =
                                //                        JsonConvert.DeserializeObject<CorrectAnswer>(
                                //                            q.Values.ToList()[1].ToString());
                                //                        if (q.Keys.ToList()[1].Contains(nextInputAnswer.InputCode))
                                //                        {
                                //                            //如果用户输入答案和标准答案不一样，则改变样式
                                //                            if (userA != null &&
                                //                                !userA.InputAnswer.Equals(iputanswer.answer))
                                //                            {
                                //                                html.Replace("contenteditable=true",
                                //                                "contenteditable=false")
                                //                                .Replace("$style",
                                //                                    @"style=""border: 1px solid rgb(210, 0, 0); font-size: 14px;""");
                                //                                html.Append(userA.InputAnswer);
                                //                            }
                                //                            else
                                //                            {
                                //                                html.Replace("contenteditable=true",
                                //                                "contenteditable=false")
                                //                                .Replace("$style", string.Empty);
                                //                                if (userA != null) html.Append(userA.InputAnswer);
                                //                            }
                                //                        }
                                //                    });
                                //                });
                                //            }
                                //            else
                                //            {
                                //                //如果此竖式任何一个空格用户输入了答案，则整个竖式禁止编辑
                                //                html.Replace("contenteditable=true",
                                //                                    "contenteditable=false")
                                //                                    .Replace("$style",
                                //                                        @"style=""border: 1px solid rgb(210, 0, 0); font-size: 14px;""");
                                //            }
                                //            l++;
                                //            if (l < inputList.Count())
                                //                //填充完一个竖式框，就移除集合中的这一项
                                //                nextInputAnswer = inputList[l];
                                //        }
                                //    }
                                //}

                                #endregion

                                html.Append("</div>");
                            }
                        }
                        html.Append("</td>");
                    }
                    html.Append("</tr>");
                }

                #region 题目的分析解答点评

                //题目的分析解答点评
                if (inputStrList.Count > 0)
                {
                    for (int jk = 0; jk < shushiTrackSeria.InputList.Count; jk++)
                    {
                        html.Replace(inputStrList[jk], "i_" + inputList[jk].InputCode);
                        shuShiCorrectAnswer?.ToList().ForEach(p =>
                        {
                            p.AnswerList.ToList().ForEach(q =>
                            {
                                CorrectAnswer correctAnswer = q.Values.ToList()[1].ToString().Json2Obj<CorrectAnswer>();
                                if (q.Keys.ToList()[1].Contains(inputList[jk].InputCode))
                                {
                                    html.Replace(
                                        string.Format(@"<div style=""display: none;"">{0}</div>",
                                            "[$]" + "i_" + inputList[jk].InputCode), correctAnswer.answer);
                                }
                            });
                        });
                    }
                }

                #endregion

                html.Append("</tbody></table>");
                return html.ToString();
            }
            catch (Exception ex)
            {
                return "竖式JSON格式错误！" + ex.Message.ToString();
            }
        }

        /// <summary>
        /// 拼接题目的答案
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private string PingChoiceHtml(Input input)
        {
            var choiceIndex = _muBuiltRequest.ChoiceIndex;
            StringBuilder csb = new StringBuilder();
            string className = "topiclist topiclist_3";
            //switch (_tiMu.OrderType)
            //{
            //    case TiMu.OrderTypeEnum.One2One: className = "topiclist topiclist_1"; break;
            //    case TiMu.OrderTypeEnum.One2Two: className = "topiclist topiclist_2"; break;
            //    case TiMu.OrderTypeEnum.One2Four: className = "topiclist topiclist_3"; break;
            //}
            string choiceType = input.InputType.Contains("checkbox") ? "checkbox" : "radio";
            csb.Append("<ul class=\"" + className + "\">");
            var inputChoiceList = input.InputChoice.OrderBy(d => d.Order).ToList();
            for (int k = 0; k < inputChoiceList.Count; k++)
            {
                var j = (k + 1).ToString();
                //将选项数字转换成A,B,C...字母
                var numToAbc = NumToABC(j);
                //如果要生成pdf试卷，则改变选项位置
                if (_muBuiltRequest.IsCreatePdf)
                {
                    csb.Append(
                        @"<li><div class=""list_Option""><div class=""li_option_titie""><input class=""input"" id=""i_$InputCode$"" name=""$InputCode$"" type=""$choiceType$"" value=""$k$"" $disabled $checked/>
                            <label class=""label"" for=""i_$InputCode$"">$numToABC$</label>
                            <label for=""i_$InputCode$"">$RegexContent$</label></div></div></li>");
                }
                else
                {
                    csb.Append(
                        @"<li><div class=""list_Option""><div class=""li_option_titie""><input class=""input"" id=""i_$InputCode$"" name=""$InputCode$"" type=""$choiceType$"" value=""$k$"" $disabled $checked/>
                            <label class=""label"" for=""i_$InputCode$"">$numToABC$</label>
                            </div><label for=""i_$InputCode$"">$RegexContent$</label></div></li>");
                }
                string inputCode = input.InputCode;
                string content = RegexContent(inputChoiceList[k].ChoiceContent);
                csb.Replace("i_$InputCode$",
                    choiceIndex != null ? inputCode + "_" + j + "_" + choiceIndex : inputCode + "_" + j)
                    .Replace("$InputCode$", inputCode)
                    .Replace("$choiceType$", choiceType)
                    .Replace("$k$", j)
                    .Replace("$RegexContent$", content);

                #region 填充用户答案

                if (_muBuiltRequest.IsShowUserAnswer)
                {
                    ////获取序列化后的答案集合
                    //var answer = _muBuiltRequest.UserResult;
                    ////当前选项在用户答案中是否查找到
                    //var isFinde = answer.Select(d => d.InputAnswer.Equals(j)).Any();
                    ////筛选用户输入答案
                    //var currentUsAnswer = answer.FirstOrDefault(d => d.InputAnswer.Split(',').Any(q => q == j));
                    ////如果是单选按钮
                    //if (choiceType == "radio" && currentUsAnswer != null)
                    //{
                    //    var isRight = currentUsAnswer?.IsRight ?? ResultEnum.IsFalse;
                    //    if (isRight.Equals(ResultEnum.IsTrue))
                    //        //正确答案
                    //        csb.Replace("$numToABC$", numToAbc + @"<dfn class=""Corrector""></dfn>")
                    //            .Replace("$checked", "checked");
                    //    else
                    //        //错误答案
                    //        csb.Replace("$numToABC$", numToAbc + @"<dfn class=""wrong""></dfn>");
                    //}
                    ////如果是多选按钮
                    //else if (choiceType == "checkbox")
                    //{
                    //    //多选，要和正确答案对比判断
                    //    var inputAnswer = input.InputAnswer.Split(',');
                    //    //筛选标准答案
                    //    var currentIpAnswer = inputAnswer.FirstOrDefault(d => d == j);
                    //    var isTrue = currentUsAnswer != null && currentIpAnswer != null && currentUsAnswer.InputAnswer == currentIpAnswer;
                    //    if (isTrue)
                    //        csb.Replace("$numToABC$", numToAbc + @"<dfn class=""Corrector""></dfn>");
                    //    else if (currentUsAnswer != null)
                    //        csb.Replace("$numToABC$", numToAbc + @"<dfn class=""wrong""></dfn>");
                    //}
                    //csb.Replace("$numToABC$", numToAbc)
                    //        .Replace("$disabled", "disabled")
                    //        .Replace("$checked", String.Empty);
                }
                else
                {
                    csb.Replace("$numToABC$", numToAbc)
                        .Replace("$disabled", string.Empty)
                        .Replace("$checked", string.Empty);
                }

                #endregion
            }
            csb.Append("</ul>");
            return csb.ToString();
        }

        /// <summary>
        /// 根据题目序号返回选项的序号（A,B,C,D...）
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string NumToABC(string num)
        {
            if (num.Trim() == string.Empty) return num;
            string[] n = num.Split(new char[] {','});
            string abc = string.Empty;
            for (int i = 0; i < n.Length; i++)
            {
                try
                {
                    abc += Convert.ToString((char) (64 + int.Parse(n[i])));
                }
                catch
                {
                    abc += n[i];
                }
            }
            return abc;
        }

        /// <summary>
        /// 根据答案类型，拼接不同类型答案显示方式
        /// </summary>
        /// <param name="content"></param>
        /// <param name="inputList"></param>
        /// <returns></returns>
        private string FormatKongStyle(string content, IList<Input> inputList)
        {
            if (inputList == null) return content;
            if (inputList.Count == 0) return content;
            StringBuilder strBuder = new StringBuilder();
            for (int i = 0; i < inputList.Count; i++)
            {
                var input = inputList[i];
                string inputCode = input.InputCode;
                string tmidUpper = input.TmId.ToString();
                string tmchildId = ""; //input.TMChildID;
                string khtml = string.Empty;
                strBuder.Clear();

                #region switch

                if (input.BaseType == "text")
                {
                    switch (input.InputType)
                    {
                        case "big-input":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""FEBox FEBoxTextArea"" type=""biginput"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        case "long-input":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""longinput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        case "hxfcs-input":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""hxfcsinput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        case "duoguo-input":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""otherinput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        case "online-input":
                            strBuder.Append(PingOnLineHtml(input, false));
                            break;
                        case "single-old":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""singleoldinput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            content = new Regex("<br/>", RegexOptions.None).Replace(content, "", 1, 0);
                            content = new Regex("\r", RegexOptions.None).Replace(content, "", 1, 0);
                            break;
                        case "single-eng":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""singleenginput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        case "single-kbd":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""singleenginput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        case "single-and":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""longinput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        case "single-sign":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""longinput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        case "simple-input":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""longinput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        case "dxs-input":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""longinput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        case "or-input":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""longinput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        case "keybox-input":
                            strBuder.Append(
                                string.Format(
                                    @"<div id=""{0}""  name=""{1}""  class=""$class"" type=""longinput"" editable=""true"" $style>$value</div>",
                                    "i_" + inputCode, inputCode));
                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrWhiteSpace(strBuder.ToString()))
                        content = new Regex("<kbd>([\\s\\S]*?)</kbd>", RegexOptions.None).Replace(content,
                            strBuder.ToString(), 1, 0);
                }
                else if (input.InputType == "radio-TF")
                {
                    strBuder.Append(
                        @"<span class=""T-or-F"" id=""k_$tmchildId$"">（<dfn id=""$tmchildId$_T""><input class=""input"" id=""i_$InputCode$_a"" name=""$InputCode$"" type=""radio"" value=""1""/>
                                    <label class=""true"" for=""i_$InputCode$"" onclick=""document.getElementById('$tmchildId$_F').className = '';document.getElementById('$tmchildId$_T').className='hot'"";\></label></dfn><dfn id=""$tmchildId$_F""><input class=""input"" id=""i_$InputCode$_b"" name =""$InputCode$"" type=""radio"" value=""2""/>
                                    <label class=""false"" for=""i_$InputCode$"" onclick=""document.getElementById('$tmchildId$_F').className = 'hot';document.getElementById('$tmchildId$_T').className = ''"";\></label></dfn>）</span>");
                    strBuder.Replace("k_$tmchildId$", "k_" + tmchildId)
                        .Replace("$InputCode$", tmidUpper.ToLower())
                        .Replace("$tmchildId$", tmchildId);
                    content = new Regex("<kbd>([\\s\\S]*?)</kbd>", RegexOptions.None).Replace(content,
                        strBuder.ToString(), 1, 0);
                }
                else
                {
                    content = new Regex("<kbd></kbd>", RegexOptions.None).Replace(content, "<kbd>" + i + "</kbd>", 1);
                }

                #endregion

                #region 获取用户答案显示

                //填充用户输入的答案，暂时排除连线题
                if (_muBuiltRequest.IsShowUserAnswer)
                {
                    //正确答案
                    //var rightAnswers = input.InputAnswer;
                    //var userAnswers = _muBuiltRequest.UserResult;
                    //if (userAnswers != null && userAnswers.Any())
                    //{
                    //    //筛选用户答案
                    //    var answer = userAnswers.FirstOrDefault(p => p.InputCode.Contains(inputCode));
                    //    var regex = RegexContent(answer != null ? answer.InputAnswer : "");//FormatAnswers(answer != null ? answer.InputAnswer : "");
                    //    if (input.InputType != "online-input" && answer != null)
                    //    {
                    //        var result = false;
                    //        if (input.InputType == "duoguo-input")
                    //        {
                    //            var inputAnswer = JsonConvert.DeserializeObject<List<TmInputAnswerDuoguoInput>>(input.InputAnswer);
                    //            inputAnswer.ForEach(p =>
                    //            {
                    //                if (p.Answer.Equals(answer.InputAnswer))
                    //                    result = true;
                    //                else
                    //                    result = false;
                    //            });
                    //        }
                    //        if (rightAnswers.Contains(answer.InputAnswer) || result)
                    //        {
                    //            //正确答案
                    //            content = content.Replace("$class", "FEBox").Replace("$style", @"style=""font-size:14px;""").Replace("$value", regex);
                    //        }
                    //        else
                    //        {
                    //            //错误答案
                    //            content = content.Replace("$class", "FEBox").Replace("$style",
                    //            @"style = ""border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(210, 0, 0); font-size: 14px;""")
                    //            .Replace("$value", regex);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        content = content.Replace("$class", "FEBox").Replace("$style", string.Empty).Replace("$value", string.Empty);
                    //    }
                    //}
                    //else
                    //{
                    //    content = content.Replace("$class", "FEBox").Replace("$style", string.Empty).Replace("$value", string.Empty);
                    //}
                }
                else
                {
                    content = content.Replace("$class", "FEBox")
                        .Replace("$style", string.Empty)
                        .Replace("$value", string.Empty);
                }

                #endregion
            }
            return content;
        }

        /// <summary>
        /// 处理答案类型是“online-input”的标签
        /// </summary>
        /// <param name="inputObj">The input object.</param>
        /// <param name="showAnswer">if set to <c>true</c> [show answer].</param>
        /// <returns></returns>
        private string PingOnLineHtml(Input inputObj, bool showAnswer)
        {
            TmInputAnswerOnlineInput tmInputAnswerOnlineInput =
                inputObj.InputAnswer.Json2Obj<TmInputAnswerOnlineInput>();
            StringBuilder onlineHtml = new StringBuilder();
            string itype = showAnswer ? "ki" : "i";
            string inputCode = inputObj.InputCode;
            onlineHtml.Append(
                string.Format(
                    @"<div class=""matching {0}"" id=""{1}"" name=""{2}"" style=""position:relative""><ul class=""matching-left"">",
                    (showAnswer ? "" : " onlineKong"), itype + "_" + inputCode, inputCode));
            foreach (string t in tmInputAnswerOnlineInput.Left)
            {
                onlineHtml.Append(string.Format(@"<li><div class=""inline"">{0}</div></li>",
                    ReplaceShuShi(RegexContent(t), null)));
            }
            onlineHtml.Append("</ul><ul class=\"matching-right\">");
            for (int i = 0; i < tmInputAnswerOnlineInput.Right.Count(); i++)
            {
                onlineHtml.Append(string.Format(@"<li><div class=""inline"">{0}</div></li>",
                    ReplaceShuShi(RegexContent(tmInputAnswerOnlineInput.Right[i]), null)));
            }
            onlineHtml.Append("</ul><div style=\"clear:both\"></div></div>");
            //连线答案
            //if (_muBuiltRequest.IsShowUserAnswer)
            //{
            //    var userAnswers = _muBuiltRequest.UserResult.FirstOrDefault(d => d.InputCode == inputObj.InputCode);
            //    if (userAnswers != null)
            //    {
            //        //处理json中的双引号问题
            //        var inputAnswer = userAnswers.InputAnswer.Replace("\"", "\\\"");
            //        StringBuilder stringBuilder = new StringBuilder();
            //        stringBuilder.Append(@"<script>$(function() { onLine.getonlinePosition(""$userAnswer"",""$inputCode"",""$isRight"",false);});</script>");
            //        stringBuilder.Replace("$userAnswer", inputAnswer)
            //        .Replace("$inputCode", userAnswers.InputCode)
            //        .Replace("$isRight", ((int)userAnswers.IsRight).ToString());
            //        onlineHtml.Append(stringBuilder);
            //    }
            //}
            return onlineHtml.ToString();
        }

        /// <summary>
        /// 根据guid获取竖式内容
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private string GetShuShiTrackData(string guid)
        {
            var timuservier = EngineContext.Current.Resolve<ITiMuService>();
            var shushitrack = timuservier.GetShushiById(Guid.Parse(guid));
            return shushitrack.Track;
        }

        /// <summary>
        /// 替换竖式
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="shuShiCorrectAnswer">The shu shi correct answer.</param>
        /// <returns></returns>
        private string ReplaceShuShi(string content, IList<ShuShiCorrectAnswer> shuShiCorrectAnswer)
        {
            //竖式样式替换
            Regex regShushikbd = new Regex("<kbd>([A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12})</kbd>",
                RegexOptions.IgnoreCase | RegexOptions.Multiline); //竖式空 
            var shushiList = _tiMu.Inputs.OrderBy(d => d.Order).Where(p => p.InputType == "shushi-input").ToList();
            int kbdcout = regShushikbd.Matches(content).Count;
            int j = 0;
            foreach (Match m in regShushikbd.Matches(content))
            {
                int pageSize = shushiList.Count/kbdcout;
                var pageList = shushiList.Skip((j + 1)*pageSize - pageSize).Take(pageSize).ToList();
                string guid = m.Groups[1].Value;
                if (guid.Trim() == string.Empty) continue;
                content = regShushikbd.Replace(content, ParserShushi(guid, pageList, shuShiCorrectAnswer), 1);
                j++;
            }
            Regex regShushi = new Regex(@"\[shushi\]([A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12})\[/shushi\]",
                RegexOptions.IgnoreCase | RegexOptions.Multiline); //竖式空 
            kbdcout = regShushi.Matches(content).Count;
            foreach (Match m in regShushi.Matches(content))
            {
                int pageSize = shushiList.Count/kbdcout;
                var pageList = shushiList.Skip((j + 1)*pageSize - pageSize).Take(pageSize).ToList();
                string guid = m.Groups[1].Value;
                if (guid.Trim() == string.Empty) continue;
                content = regShushi.Replace(content, ParserShushi(guid, pageList, shuShiCorrectAnswer), 1);
                j++;
            }
            return content;
        }

        #endregion

        #region 生成题目的分析解答点评 （判分 反思 拓展练习 微课...）

        /// <summary>
        /// 生成分析解答点评的菜单
        /// </summary>
        /// <param name="tmidToUpper"></param>
        /// <returns></returns>
        private string TiMuAnalyse(string tmidToUpper)
        {
            //<li><dfn class=""Analytic_icon_05""></dfn><a href=""javascript:; "" class=""button"" id=""btuozhan_[$id]"">拓展练习</a></li>
            //< li >< dfn class=""Analytic_icon-weike""></dfn><a href = ""javascript:; "" class=""button"" id=""bvideo_[$id]"" onclick=""window.login.startFunction();"">微课</a></li>

            #region Analytic_Top

            string tjV = "题解", daV = "答案", scV = "收藏", fsV = "反思", bjV = "编辑", dpV = "评语";
            var isbrowse = "0";
            var analyseIsEmpty = "1";
            var isDisplay = "style=display:block;";
            StringBuilder sbAnalyse = new StringBuilder();
            sbAnalyse.Append(@"<div class=""Analytic_Top"" id=""analytic_[$id]"">
                    <ul>
                        <li [$isDisplay]><dfn class=""Analytic_icon_01""></dfn><a href=""javascript:; "" class=""button"" id=""banalyse_[$id]"" [$isDisabled]>[$tjV]</a></li>
                        <li [$isDisplay]><dfn class=""Analytic_icon_02""></dfn><a href=""javascript:; "" class=""button"" id=""bjudge_[$id]"" [$isbrowse]>[$daV]</a></li>
                    </ul>
                <div class=""clear""></div> 
                </div>
                <div class=""Analytic_Bottom"" id=""root_[$id]"" style=""display:none"" curObj=""""></div>
            ");
            sbAnalyse.Replace("[$id]", tmidToUpper).Replace("[$isDisplay]", isDisplay)
                .Replace("[$isDisabled]",
                    ((Convert.ToInt32(analyseIsEmpty) == 1 || Convert.ToInt32(isbrowse) == 0) ? "" : "disabled = 'true'"))
                .Replace("[$isbrowse]", (Convert.ToInt32(isbrowse) == 0 ? "" : "disabled = 'true'"))
                .Replace("[$tjV]", tjV).Replace("[$daV]", daV)
                .Replace("[$Video]", "");

            #endregion

            return sbAnalyse.ToString();
        }

        /// <summary>
        /// 生成题目的分析解答点评
        /// </summary>
        /// <param name="analyseName">Name of the analyse.</param>
        /// <returns></returns>
        public IList<string> PingTiMuAnalyse(string analyseName)
        {
            var tmid = _tiMu.Id.ToString();
            IList<string> strList = new List<string>();
            string tmidToUpper = _tiMu.Id.ToString().ToUpper();
            StringBuilder sbAnalyse = new StringBuilder();
            switch (analyseName)
            {
                case "banalyse":

                    #region Analytic_Bottom

                    Regex reg = new Regex("<(?!img|embed|kbd).*?>|\\t",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    string analyse = string.IsNullOrWhiteSpace(_tiMu.Analysis) ? "" : _tiMu.Analysis;
                    string explain = string.IsNullOrWhiteSpace(_tiMu.Answer) ? "" : _tiMu.Answer;
                    string comment = string.IsNullOrWhiteSpace(_tiMu.Comment) ? "" : _tiMu.Comment;

                    string Diff = string.IsNullOrWhiteSpace(_tiMu.Difficult.ToString())
                        ? ""
                        : _tiMu.Difficult.ToString();
                    string strDiff = string.Empty;
                    switch (Diff)
                    {
                        case "1":
                        case "2":
                            strDiff = "容易题";
                            break;
                        case "3":
                            strDiff = "较容易题";
                            break;
                        case "4":
                        case "5":
                            strDiff = "较难题";
                            break;
                        default:
                            break;
                    }
                    sbAnalyse.Append(@"<html xmlns:mml=""http://www.w3.org/1998/Math/MathML"">");
                    sbAnalyse.Append(@"<div class=""Analytic_body"">");
                    if (reg.Replace(analyse.Trim(), "").Replace("&nbsp;", " ").Trim() != string.Empty)
                    {
                        sbAnalyse.Append(@"<div class=""Analytic_section""><em>【分析】</em>[$RegexAnalyse]</div>");
                    }
                    if (reg.Replace(explain.Trim(), "").Replace("&nbsp;", " ").Trim() != string.Empty)
                    {
                        sbAnalyse.Append(@"<div class=""Analytic_section""><em>【解答】</em>[$RegexExplain]</div>");
                    }
                    if (reg.Replace(comment.Trim(), "").Replace("&nbsp;", " ").Trim() != string.Empty)
                    {
                        sbAnalyse.Append(@"<div class=""Analytic_section""><em>【点评】</em>[$RegexComment]</div>");
                    }
                    sbAnalyse.Append(@"</div>");
                    sbAnalyse.Append(@"</html>");
                    sbAnalyse.Replace("&nbsp;", "&#160;").Replace("<mml:", "<").Replace("</mml:", "</");
                    sbAnalyse.Append(@"<div class=""KPDiff"">
                    <p><em>【考点】</em>[$kdContent]</p>
                    <p><em>【难度】</em>[$strDiff]</p></div>");

                    #endregion

                    //获取题目主要的知识点
                    var knowledgeList =
                        _tiMu.Subject.Knowledges.Where(
                            p => _tiMu.Knowledges.Where(q => q.IsMain).Select(q => q.KnowledgeId).Contains(p.Id))
                            .ToList();
                    var knowledge = string.Empty;
                    knowledgeList.ToList().ForEach(p =>
                    {
                        if (knowledgeList.Count() > 1)
                            knowledge += p.Name + ",";
                        else
                            knowledge += p.Name;
                    });

                    sbAnalyse.Replace("[$RegexAnalyse]", RegexContent(analyse.Trim()))
                        .Replace("[$RegexExplain]", RegexContent(explain.Trim()))
                        .Replace("[$RegexComment]", RegexContent(comment.Trim()))
                        .Replace("[$kdContent]", knowledge)
                        .Replace("[$strDiff]", strDiff);

                    break;
                case "bjudge":

                    #region

                    int index = 1;
                    sbAnalyse.Append(@"<div class=""Analytic-table-box"" id=""result_[$id]"">
                            <table cellpadding=""0"" cellspacing=""0"" class=""Analytic-table"">
                                <tr>
                                    <th>序号</th>
                                    <th>参考答案</th>
                                    <th class=""_OnOffTemp_"">判分</th>
                                </tr>
                                [$content]
                            </table>
                        </div>");
                    StringBuilder analyses = new StringBuilder();
                    //如果标答案型是竖式，特殊处理
                    var analyseList = _tiMu.Inputs.OrderBy(p => p.Order).ToList();
                    var shushilist = analyseList.Where(p => p.InputType == "shushi-input").ToList();
                    if (shushilist.Any())
                    {
                        var distinct = shushilist.Select(p => p.InputAnswer).Distinct().ToList();
                        int pageSize = shushilist.Count/distinct.Count;
                        for (int i = 0; i < distinct.Count; i++)
                        {
                            analyses.Append(@"<tr><td>[$index]</td>
                                    <td>[$Replace]</td>
                                    <td class=""_OnOffTemp_"">[$inputScore]</td>
                                </tr>");
                            var pageList = shushilist.Skip((i + 1)*pageSize - pageSize).Take(pageSize).ToList();
                            IList<ShuShiCorrectAnswer> shuShiCorrectAnswer =
                                distinct[i].Json2Obj<IList<ShuShiCorrectAnswer>>();
                            var shushi = ParserShushi(shuShiCorrectAnswer[0].Guid, pageList, shuShiCorrectAnswer);
                            analyses.Replace("[$index]", (index++).ToString())
                                .Replace("[$Replace]", shushi)
                                .Replace("[$inputScore]", shushilist.Sum(p => p.Score) + "分");
                        }
                        shushilist.ForEach(p => { analyseList.Remove(p); });
                    }
                    foreach (var item in analyseList)
                    {
                        var inputAnswer = item.InputAnswer;
                        analyses.Append(@"<tr><td>[$index]</td>
                                    <td>[$Replace]</td>
                                    <td class=""_OnOffTemp_"">[$inputScore]</td>
                                </tr>");
                        switch (item.InputType)
                        {
                            case "online-input":
                                analyses.Replace("[$Replace]", PingOnLineHtml(item, true))
                                    .Replace("[$inputScore]", item.Score + "分");
                                break;
                            case "radio":
                                analyses.Replace("[$Replace]", NumToABC(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "duoguo-input":
                                List<TmInputAnswerDuoguoInput> tmInputAnswer =
                                    inputAnswer.Json2Obj<List<TmInputAnswerDuoguoInput>>();
                                analyses.Replace("[$Replace]", RegexContent(tmInputAnswer[0].Answer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "checkbox-all":
                                analyses.Replace("[$Replace]", NumToABC(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "checkbox":
                                analyses.Replace("[$Replace]", NumToABC(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "radio-cite":
                                analyses.Replace("[$Replace]", NumToABC(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "simple-input":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "dxs-input":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "radio-TF":
                                analyses.Replace("[$Replace]",
                                    NumToABC(inputAnswer) == "A"
                                        ? @"<span class=""TorF-answer TorF-answer-T""></span>"
                                        : @"<span class=""TorF-answer TorF-answer-F""></span>")
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "or-input":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "big-input":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "hxfcs-input":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "long-input":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "single-eng":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "single-kbd":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "single-old":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "single-and":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "keybox-input":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            case "single-sign":
                                analyses.Replace("[$Replace]", RegexContent(inputAnswer))
                                    .Replace("[$inputScore]", @"<div class=""longJudge""><em class=""hot""></em></div>");
                                    //todo
                                break;
                            default:
                                break;
                        }
                        analyses.Replace("[$index]", (index++).ToString());
                    }
                    sbAnalyse.Replace("[$content]", analyses.ToString());
                    break;

                    #endregion
            }
            strList.Add(sbAnalyse.ToString());
            strList.Add(_tiMu.Inputs.ToList()[0].InputType);
            strList.Add(_tiMu.Inputs.ToList()[0].InputAnswer);
            strList.Add(_tiMu.Inputs.ToList()[0].InputCode);
            return strList;
        }

        #endregion

        #region 根据题目Id集合获取用户输入的答案        

        /// <summary>
        /// 格式化用户输入的答案
        /// </summary>
        /// <param name="answerStr">The answer string.</param>
        /// <returns></returns>
        private string FormatAnswers(string answerStr)
        {
            //正则表达式替换``中的公式内容
            Regex regex = new Regex(@"`(.*?)`");
            MatchCollection listTm = regex.Matches(answerStr);
            for (int i = 0; i < listTm.Count; i++)
            {
                var gs = listTm[i].Value;
                string img =
                    string.Format(
                        @"<img class=""mathmlImg"" onerror=""this.src = '{0}'"";this.onerror = null; src='{0}'/>",
                        _mathmlImg.Replace("$url$", gs));
                answerStr = answerStr.Replace(listTm[i].Value, img);
            }
            //竖式连线  {"code":"LY_201_UAA_1000";"answer":["0|-1","1|-1","2|-1"]}
            Regex regexOline = new Regex("{\"answer\":(.*?)}");
            bool isOline = regexOline.IsMatch(answerStr);
            if (isOline)
                answerStr = "$" + answerStr; //连线特殊标记
            //<:&gt
            answerStr = answerStr.Replace("&gt", "<");
            return answerStr;
        }

        #endregion

        #region 封装解析后的题目信息到实体对象TiMuModel

        /// <summary>
        /// 准备解析后的题目信息
        /// </summary>
        /// <returns></returns>
        public TiMuModel PrepareTiMuModel(TiMu tiMu)
        {
            var tiMuModel = new TiMuModel();
            //j解析题干
            string oldtimu = tiMu.Trunk;
            oldtimu = ReplaceShuShi(oldtimu, null);
            if (tiMu.Inputs.Count != 0)
            {
                var inputList = tiMu.Inputs.OrderBy(d => d.Order).ToList();
                oldtimu = RegexContent(FormatKongStyle(oldtimu, inputList));
                //正则替换kbd
                Regex regexkbd = new Regex("<kbd>([\\s\\S]*?)</kbd>");
                //判断题目答案类型 题目类型
                if (inputList.Count > 0)
                {
                    for (int i = 0; i < inputList.Count; i++)
                    {
                        var input = inputList.ToList()[i];
                        if (inputList[i].BaseType == "choice" && inputList[i].InputType != "radio-TF")
                        {
                            string replaceHtml = PingChoiceHtml(inputList[i]);

                            oldtimu = regexkbd.Replace(oldtimu, replaceHtml);
                        }
                        else
                        {
                            oldtimu = regexkbd.Replace(oldtimu,
                                string.Format(
                                    @"<div class=""FEBox"" id=""i_{0}"" name=""{0}"" type=""otherinput"" editable=""true""></div>",
                                    input.InputCode), 1, 0);
                        }
                    }
                }
            }
            tiMuModel.Tmid = tiMu.Id;
            tiMuModel.Trunk = oldtimu;
            tiMuModel.TiMuTypeName = tiMu.TiMuType.Name;
            tiMuModel.Analysis = RegexContent(tiMu.Analysis);
            tiMuModel.Answer = RegexContent(tiMu.Answer);
            tiMuModel.Comment = tiMu.Comment;
            tiMuModel.Difficult = tiMu.Difficult;
            tiMuModel.Status = tiMu.BookTiMu.Status;
            //获取主知识点
            var firstOrDefault = tiMu.Knowledges.FirstOrDefault(d => d.IsMain);
            if (firstOrDefault != null && firstOrDefault.KnowledgePiont != null)
                tiMuModel.MainKnowledgeId =
                    firstOrDefault.KnowledgePiont.Name;
            //相关知识点
            var minors = tiMu.Knowledges.Where(d => !d.IsMain).Select(d => d.KnowledgePiont).ToList();
            var minor = "";
            if (minors.Any())
            {
                minors.ForEach(d =>
                {
                    if (d != null)
                    {
                        minor += d.Name + ",";
                    }
                });
            }
            tiMuModel.MinorKnowledgeId = minor.TrimEnd(',');
            //标记错误信息
            tiMuModel.ErrorMessage = tiMu.BookTiMu.ErrorInfo;
            tiMuModel.VideoCode = tiMu.VideoCode;
            //排序
            tiMuModel.Order = tiMu.BookTiMu.Order;
            return tiMuModel;
        }

        /// <summary>
        /// 拼装解析题目、子题目
        /// </summary>
        /// <returns></returns>
        public TiMuModel AssemblyTimu()
        {
            //取父题目
            var tiMuModel = PrepareTiMuModel(_tiMu);
            var timuChildList = new List<TiMuModel>();
            //遍历取出所有子题目
            for (int i = 0; i < _tiMu.TiMuChild.Count; i++)
            {
                var childTimu = _tiMu.TiMuChild.ToList()[i];
                timuChildList.Add(PrepareTiMuModel(childTimu));
            }
            tiMuModel.ChildTimuList = timuChildList;
            return tiMuModel;
        }

        #endregion

        #region Md5

        /// <summary>
        /// Gets the MD5 hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static string GetMd5Hash(string input)
        {
            var md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        #endregion
    }
}