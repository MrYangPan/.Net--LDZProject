using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using AF.Core.Extensions;
using AF.Core.Infrastructure;
using AF.Domain.Domain.TiMus;
using Newtonsoft.Json;

namespace AF.Services.TiMus.TiMuCompose
{

    /// <summary>
    /// 题目录入
    /// </summary>
    public class TiMuEntry
    {
        #region Fields
        private readonly TimuJsonModel _timuJsonModel;
        private readonly TiMu _timu;

        /// <summary>
        /// Initializes a new instance of the <see cref="TiMuEntry" /> class.
        /// </summary>
        /// <param name="timuJsonModel">The timu json model.</param>
        /// <param name="tiMu">The ti mu.</param>
        public TiMuEntry(TimuJsonModel timuJsonModel,TiMu tiMu)
        {
            _timuJsonModel = timuJsonModel;
            //获取题目对象
            _timu = tiMu;
            /* GeTiMuById(_timuJsonModel.TiMuId);*/
        }
        #endregion

        #region 题目解析（选项、分析、解答、点评等）

        /// <summary>
        /// 录题页面编辑使用
        /// </summary>
        /// <returns></returns>
        public static void EntryGeTiMuFormatById(TiMu timu)
        {
            var inputs = timu.Inputs;
            if (inputs.Any())
            {
                var reg = new Regex("<kbd></kbd>");
                var inputList = inputs.OrderBy(d => d.Order).ToList();
                for (int i = 0; i < inputList.Count; i++)
                {
                    var type = inputList[i].InputType;
                    switch (type)
                    {
                        case "radio":
                            timu.Trunk = timu.Trunk.Replace("<kbd></kbd>", "");
                            break;

                        case "checkbox-all":
                            timu.Trunk = timu.Trunk.Replace("<kbd></kbd>", "");
                            break;

                        case "online-input":
                            var online = "[k type=\"" + type + "\" score=\"" + inputList[i].InputScore + "\"]" + inputList[i].InputAnswer + "[/k]";
                            timu.Trunk = reg.Replace(timu.Trunk, online, 1, 0);
                            break;

                        case "duoguo-input":
                            var duoguo = "[k type=\"" + type + "\" score=\"" + inputList[i].InputScore + "\"]" + inputList[i].InputAnswer + "[/k]";
                            timu.Trunk = reg.Replace(timu.Trunk, duoguo, 1, 0);
                            break;
                        default:
                            var others = "[k type=\"" + type + "\" score=\"" + inputList[i].InputScore + "\"]" + inputList[i].InputAnswer + "[/k]";
                            timu.Trunk = reg.Replace(timu.Trunk, others, 1, 0);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 题目录入，不同类型的题干和答案解析
        /// </summary>
        /// <returns></returns>
        public AnswerResult TiMuFormat()
        {
            var regKong = new Regex("\\[k.*?type=\"(.*?)\"[\\s\\S]*?score=\"([0-9.]+)\"]([\\s\\S]*?)\\[/k\\]", RegexOptions.Multiline);
            var trunk = "";//题干
            var inputs = new List<Input>();
            var inputChoices = new List<InputChoice>();
            var deleteInputIds = new List<Input>();//保存要被删除的input
            switch (_timuJsonModel.ChoiceType)
            {
                #region 单选
                case (int)ChoiceType.RdoRdo:
                    trunk = _timuJsonModel.TiGan + "<kbd></kbd>";
                    //题目
                    _timu.Trunk = trunk;
                    _timu.Analysis = _timuJsonModel.Analyse;
                    _timu.Answer = _timuJsonModel.Explain;
                    _timu.Comment = _timuJsonModel.Comment;
                    _timu.TiMuTypeId = _timuJsonModel.TimuType;
                    _timu.Year = DateTime.Now.Year;
                    //判断是否有答案，没有就创建对象，有就修改
                    if (_timu.Inputs.Any())
                    {
                        var inputR = _timu.Inputs.ToList()[0];
                        //答案
                        inputR.InputType = "radio";
                        inputR.InputCode = _timu.Id.ToString();
                        inputR.InputAnswer = AlphabetToNumber(_timuJsonModel.ChoicesIput.RightAnswer);
                        inputR.Score = decimal.Parse(_timuJsonModel.ChoicesIput.Score.ToString());
                        inputR.Order = 1;
                        inputR.BaseType = "choice";
                        var choices = _timuJsonModel.ChoicesIput.InputChoice;
                        //判断是否有选项，没有就创建对象，有就修改
                        if (_timu.Inputs.ToList()[0].InputChoice.Any())
                        {
                            var choiceList = _timu.Inputs.ToList()[0].InputChoice.OrderBy(d => d.Order).ToList();

                            for (int i = 0; i < choiceList.Count; i++)
                            {
                                var choice = choiceList[i];
                                choice.ChoiceContent = choices[i].ChoiceContent;
                                choice.Order = choices[i].Order;
                                //添加选项到集合
                                inputChoices.Add(choice);
                            }
                            if (choices.Count > choiceList.Count)
                            {
                                //新增选项
                                for (int i = choiceList.Count; i < choices.Count; i++)
                                {
                                    var choice = new InputChoice()
                                    {
                                        Id = Guid.NewGuid(),
                                        InputId = inputR.Id,
                                        ChoiceContent = choices[i].ChoiceContent,
                                        Score = decimal.Parse("0"),
                                        Order = i + 1
                                    };
                                    //添加选项到集合
                                    inputChoices.Add(choice);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < choices.Count; i++)
                            {
                                var choice = new InputChoice()
                                {
                                    Id = Guid.NewGuid(),
                                    InputId = inputR.Id,
                                    ChoiceContent = choices[i].ChoiceContent,
                                    Score = decimal.Parse("0"),
                                    Order = i + 1
                                };
                                //添加选项到集合
                                inputChoices.Add(choice);
                            }
                        }
                        //添加答案到集合
                        inputs.Add(inputR);
                    }
                    else
                    {
                        var inputR = new Input()
                        {
                            //答案
                            Id = Guid.NewGuid(),
                            TmId = _timu.Id,
                            InputCode = _timu.Id.ToString(),
                            InputType = "radio",
                            BaseType = "choice",
                            InputAnswer = AlphabetToNumber(_timuJsonModel.ChoicesIput.RightAnswer),
                            Score = decimal.Parse(_timuJsonModel.ChoicesIput.Score.ToString()),
                            Order = 1
                        };
                        var choiceList = _timuJsonModel.ChoicesIput.InputChoice.OrderBy(d => d.Order).ToList();
                        for (int i = 0; i < choiceList.Count; i++)
                        {
                            var choice = new InputChoice()
                            {
                                Id = Guid.NewGuid(),
                                InputId = inputR.Id,
                                ChoiceContent = choiceList[i].ChoiceContent,
                                Score = decimal.Parse("0"),
                                Order = i + 1
                            };
                            //添加选项到集合
                            inputChoices.Add(choice);
                        }
                        //添加答案到集合
                        inputs.Add(inputR);
                    }
                    break;
                #endregion

                #region 多选
                case (int)ChoiceType.RdoCk:
                    trunk = _timuJsonModel.TiGan + "<kbd></kbd>";
                    //题目
                    _timu.Trunk = trunk;
                    _timu.Analysis = _timuJsonModel.Analyse;
                    _timu.Answer = _timuJsonModel.Explain;
                    _timu.Comment = _timuJsonModel.Comment;
                    _timu.TiMuTypeId = _timuJsonModel.TimuType;
                    _timu.Year = DateTime.Now.Year;
                    //判断是否有答案，没有就创建对象，有就修改
                    if (_timu.Inputs.Any())
                    {
                        var inputR = _timu.Inputs.ToList()[0];
                        //答案
                        inputR.InputType = "checkbox-all";
                        inputR.BaseType = "choice";
                        inputR.InputCode = _timu.Id.ToString();
                        inputR.InputAnswer = AlphabetToNumber(_timuJsonModel.ChoicesIput.RightAnswer);
                        inputR.Score = decimal.Parse(_timuJsonModel.ChoicesIput.Score.ToString());
                        inputR.Order = 1;
                        var choices = _timuJsonModel.ChoicesIput.InputChoice;
                        //判断是否有选项，没有就创建对象，有就修改
                        if (_timu.Inputs.ToList()[0].InputChoice.Any())
                        {
                            var choiceList = _timu.Inputs.ToList()[0].InputChoice.OrderBy(d => d.Order).ToList();

                            for (int i = 0; i < choiceList.Count; i++)
                            {
                                var choice = choiceList[i];
                                choice.ChoiceContent = choices[i].ChoiceContent;
                                choice.Order = choices[i].Order;
                                //添加选项到集合
                                inputChoices.Add(choice);
                            }
                            if (choices.Count > choiceList.Count)
                            {
                                //新增选项
                                for (int i = choiceList.Count; i < choices.Count; i++)
                                {
                                    var choice = new InputChoice()
                                    {
                                        Id = Guid.NewGuid(),
                                        InputId = inputR.Id,
                                        ChoiceContent = choices[i].ChoiceContent,
                                        Score = decimal.Parse("0"),
                                        Order = i + 1
                                    };
                                    //添加选项到集合
                                    inputChoices.Add(choice);
                                }
                            }
                            //添加答案到集合
                            inputs.Add(inputR);
                        }
                        else
                        {
                            for (int i = 0; i < choices.Count; i++)
                            {
                                var choice = new InputChoice()
                                {
                                    Id = Guid.NewGuid(),
                                    InputId = inputR.Id,
                                    ChoiceContent = choices[i].ChoiceContent,
                                    Score = decimal.Parse("0"),
                                    Order = i + 1
                                };
                                //添加选项到集合
                                inputChoices.Add(choice);
                            }
                        }
                    }
                    else
                    {
                        var inputC = new Input()
                        {
                            //答案
                            Id = Guid.NewGuid(),
                            TmId = _timu.Id,
                            InputCode = _timu.Id.ToString(),
                            InputType = "checkbox-all",
                            BaseType = "choice",
                            InputAnswer = AlphabetToNumber(_timuJsonModel.ChoicesIput.RightAnswer),
                            Score = decimal.Parse(_timuJsonModel.ChoicesIput.Score.ToString()),
                            Order = 1
                        };
                        var choiceList = _timuJsonModel.ChoicesIput.InputChoice.OrderBy(d => d.Order).ToList();
                        for (int i = 0; i < choiceList.Count; i++)
                        {
                            var choice = new InputChoice()
                            {
                                Id = Guid.NewGuid(),
                                InputId = inputC.Id,
                                ChoiceContent = choiceList[i].ChoiceContent,
                                Score = decimal.Parse("0"),
                                Order = i + 1
                            };
                            //添加选项到集合
                            inputChoices.Add(choice);
                        }
                        //添加答案到集合
                        inputs.Add(inputC);
                    }
                    break;
                #endregion

                #region 判断
                case (int)ChoiceType.RdoTf:
                    _timu.Trunk = _timuJsonModel.TiGan;
                    _timu.Analysis = _timuJsonModel.Analyse;
                    _timu.Answer = _timuJsonModel.Explain;
                    _timu.Comment = _timuJsonModel.Comment;
                    _timu.TiMuTypeId = _timuJsonModel.TimuType;
                    _timu.Year = DateTime.Now.Year;
                    //如果已经存在用户答案
                    if (_timu.Inputs.Any())
                    {
                        var input = _timu.Inputs.ToList()[0];
                        input.InputAnswer = AlphabetToNumber(_timuJsonModel.ChoicesIput.RightAnswer);
                        input.Score = decimal.Parse(_timuJsonModel.ChoicesIput.Score.ToString());
                        input.InputCode = _timu.Id + "_a";
                        input.BaseType = "choice";
                        input.InputType = "radio-TF";
                        var choices = _timuJsonModel.ChoicesIput.InputChoice;
                        //如果有选项
                        if (_timu.Inputs.ToList()[0].InputChoice.Any())
                        {
                            var choiceList = _timu.Inputs.ToList()[0].InputChoice.OrderBy(d => d.Order).ToList();
                            for (int i = 0; i < choices.Count; i++)
                            {
                                var choice = choiceList[i];
                                choice.ChoiceContent = choices.OrderBy(d => d.Order).ToList()[i].ChoiceContent;
                                //添加选项到集合
                                inputChoices.Add(choice);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < choices.Count(); i++)
                            {
                                var choice = new InputChoice()
                                {
                                    Id = Guid.NewGuid(),
                                    InputId = input.Id,
                                    ChoiceContent = choices[i].ChoiceContent,
                                    Score = 0,
                                    Order = choices[i].Order
                                };
                                //添加选项到集合
                                inputChoices.Add(choice);
                            }
                        }
                        //添加答案到集合
                        inputs.Add(input);
                    }
                    else
                    {
                        var input = new Input()
                        {
                            Id = Guid.NewGuid(),
                            TmId = _timu.Id,
                            InputCode = _timu.Id + "_a",
                            BaseType = "choice",
                            InputType = "radio-TF",
                            InputAnswer = AlphabetToNumber(_timuJsonModel.ChoicesIput.RightAnswer),
                            Score = decimal.Parse(_timuJsonModel.ChoicesIput.Score.ToString()),
                            Order = 1,
                            InputScore = null
                        };
                        var choiceList = _timuJsonModel.ChoicesIput.InputChoice.OrderBy(d => d.Order).ToList();
                        for (int i = 0; i < choiceList.Count(); i++)
                        {
                            var choice = new InputChoice()
                            {
                                Id = Guid.NewGuid(),
                                InputId = input.Id,
                                ChoiceContent = choiceList[i].ChoiceContent,
                                Score = 0,
                                Order = choiceList[i].Order,
                            };
                            //添加选项到集合
                            inputChoices.Add(choice);
                        }
                        //添加答案到集合
                        inputs.Add(input);
                    }
                    break;
                #endregion

                #region 填空
                case (int)ChoiceType.RdoText:
                    _timu.Analysis = _timuJsonModel.Analyse;
                    _timu.Answer = _timuJsonModel.Explain;
                    _timu.Comment = _timuJsonModel.Comment;
                    _timu.TiMuTypeId = _timuJsonModel.TimuType;
                    _timu.Year = DateTime.Now.Year;
                    var matchCollection = regKong.Matches(_timuJsonModel.TiGan);
                    if (matchCollection.Count < _timu.Inputs.Count)
                    {
                        //先更新
                        for (int i = 1; i <= matchCollection.Count; i++)
                        {
                            ClassiFication(matchCollection[i - 1], i, inputs, "update");
                        }
                        //删除
                        deleteInputIds = DeleteInput(inputs);
                    }
                    else
                    {
                        //先更新
                        for (int i = 1; i <= _timu.Inputs.Count; i++)
                        {
                            ClassiFication(matchCollection[i - 1], i, inputs, "update");
                        }
                        if (matchCollection.Count > _timu.Inputs.Count)
                        {
                            //添加
                            for (int i = _timu.Inputs.Count; i < matchCollection.Count; i++)
                            {
                                ClassiFication(matchCollection[i], i + 1, inputs, "add");
                            }
                        }
                    }
                    for (int i = 0; i < matchCollection.Count; i++)
                    {
                        var type = matchCollection[i].Groups[1];
                        var score = matchCollection[i].Groups[2];
                        var text = matchCollection[i].Groups[3];
                        if (type.ToString() == "shushi")
                        {
                            _timuJsonModel.TiGan = regKong.Replace(_timuJsonModel.TiGan, "<kbd>" + text + "</kbd>", 1, 0);
                        }
                        else
                        {
                            _timuJsonModel.TiGan = regKong.Replace(_timuJsonModel.TiGan, "<kbd></kbd>", 1, 0);
                        }
                    }
                    _timu.Trunk = _timuJsonModel.TiGan;
                    break;
                    #endregion
            }
            return new AnswerResult() { TiMu = _timu, InputList = inputs, InputChoiceList = inputChoices, DeleteInputs = deleteInputIds };
        }

        /// <summary>
        /// 字母转换成数字，返回逗号分隔字符串（主要用于单选、多选的正确答案）
        /// </summary>
        /// <param name="rightAnswer">The right answer.</param>
        /// <returns></returns>
        private string AlphabetToNumber(string rightAnswer)
        {
            var str = "";
            for (int i = 0; i < rightAnswer.Length; i++)
            {
                var a = (short)(Encoding.ASCII.GetBytes("A")[0]);
                var bt = Encoding.ASCII.GetBytes(rightAnswer[i].ToString().ToUpper());
                var asciicode = (short)(bt[0]);
                var math = asciicode % (Convert.ToInt32(a) - 1);
                if (str == "")
                {
                    str = math.ToString();
                }
                else
                {
                    str = str + "," + math;
                }
            }
            return str;
        }

        /// <summary>
        /// 特殊处理竖式题
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="order">The order.</param>
        /// <param name="inputList">The input list.</param>
        /// <param name="operating">The operating.</param>
        private void ClassiFication(Match match, int order, List<Input> inputList, string operating)
        {
            var input = new Input();
            var inputType = match.Groups[1].ToString();
            var inputScore = match.Groups[2].ToString();
            var inputAnswer = match.Groups[3].ToString();
            switch (inputType)
            {
                case "duoguo-input":
                    InsideAddKong("duoguo-input", inputScore, inputAnswer, operating, order, inputList);
                    break;

                case "online-input":
                    InsideAddKong("online-input", inputScore, inputAnswer, operating, order, inputList);
                    break;

                case "shushi":
                    if (inputAnswer.Length >= 36)
                    {
                        //根据GUID获取竖式内容	
                        var guid = inputAnswer;
                        var track = GetShuShiTrackData(guid);
                        if (string.IsNullOrEmpty(track)) return;
                        InsideAddKong("shushi-input", inputScore, inputAnswer, operating, order, inputList);
                        //反序列化竖式Json
                        ShushiTrackSeria shushiTrackSeria = track.Json2Obj<ShushiTrackSeria>();
                        var gkongList = new List<GKong>();
                        for (int i = 0; i < shushiTrackSeria.InputList.Count; i++)
                        {
                            var diction = new Dictionary<string, AnswerList>();
                            var shushiAnswer = shushiTrackSeria.InputList[i].Answer;
                            var score = Convert.ToInt32(shushiTrackSeria.Score) / shushiTrackSeria.InputList.Count;
                            diction.Add(key: _timu.Id + "_" + order, value: new AnswerList() { Answer = shushiAnswer, Score = score });
                            var gkong = new GKong();
                            if (_timuJsonModel.ChoicesIput.InputChoice.Any())
                            {
                                gkong.Order = i;
                                gkong.AnswerList = diction;
                            }
                            else
                            {
                                gkong.Order = _timuJsonModel.ChoicesIput.InputChoice[i].Order + i + 1;
                                gkong.AnswerList = diction;
                            }
                            gkongList.Add(gkong);
                        }
                        var groupKongAnswer = new GroupKongAnswer()
                        {
                            Guid = Guid.Parse(guid),
                            GKong = gkongList,
                        };
                        foreach (InputIListChild inputChild in shushiTrackSeria.InputList)
                        {
                            inputType = "shushi-input";
                            inputScore = (Convert.ToInt32(shushiTrackSeria.Score) / shushiTrackSeria.InputList.Count).ToString();
                            inputAnswer = groupKongAnswer.Obj2Json();
                            InsideAddKong(inputType, inputScore, inputAnswer, operating, order, inputList);
                            _timu.Inputs.ToList()[_timu.Inputs.Count - 1].Id = Guid.Parse(guid);
                        }
                    }
                    break;

                default:
                    InsideAddKong(inputType, inputScore, inputAnswer, operating, order, inputList);
                    break;
            }
        }

        /// <summary>
        /// 提取的公用方法
        /// </summary>
        /// <param name="inputType">Type of the input.</param>
        /// <param name="inputScore">The input score.</param>
        /// <param name="inputAnswer">The input answer.</param>
        /// <param name="operating">The operating.</param>
        /// <param name="inputOrder">The input order.</param>
        /// <param name="inputList">The input list.</param>
        private void InsideAddKong(string inputType, string inputScore, string inputAnswer, string operating, int inputOrder, List<Input> inputList)
        {
            if (operating == "update")
            {
                var input = _timu.Inputs.ToList()[inputOrder - 1];
                input.InputType = inputType;
                input.InputCode = _timu.Id + "_" + (char)(inputOrder + 96);
                input.InputScore = inputScore;
                input.InputAnswer = inputAnswer;
                input.Order = inputOrder;
                input.BaseType = "text";
                inputList.Add(input);
            }
            else if (operating == "add")
            {
                var input = new Input()
                {
                    Id = Guid.NewGuid(),
                    TmId = _timu.Id,
                    Order = inputOrder,
                    InputCode = _timu.Id + "_" + (char)(inputOrder + 96),
                    InputType = inputType,
                    InputScore = inputScore,
                    InputAnswer = inputAnswer,
                    BaseType = "text"
                };
                inputList.Add(input);
            }
        }

        /// <summary>
        /// 删除答案
        /// </summary>
        /// <param name="inputList">The input list.</param>
        /// <returns></returns>
        private List<Input> DeleteInput(List<Input> inputList)
        {
            if (inputList.Any())
            {
                var uids = inputList.Select(d => d.Id);
                //获取要被删除的答案id集合
                var dids = _timu.Inputs.Where(d => !uids.Contains(d.Id)).ToList();
                return dids;
            }
            return null;
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
        /// 根据guid获取题目对象
        /// </summary>
        /// <param name="tmid">The tmid.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private TiMu GeTiMuById(Guid tmid)
        {
            var timuservier = EngineContext.Current.Resolve<ITiMuService>();
            return timuservier.GeTiMuById(tmid);
        }

        #endregion

    }
}
