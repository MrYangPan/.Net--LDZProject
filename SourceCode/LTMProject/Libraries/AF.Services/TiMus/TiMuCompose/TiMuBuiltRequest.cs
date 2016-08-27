using System.Collections.Generic;
using AF.Domain.Domain.TiMus;
using Newtonsoft.Json;

namespace AF.Services.TiMus.TiMuCompose
{
    public class TiMuBuiltRequest
    {

        public TiMu TiMu { get; set; }

        /// <summary>
        /// 是否显示题解导航菜单
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is analytic visable; otherwise, <c>false</c>.
        /// </value>
        public bool IsAnalyticVisable { get; set; }

        /// <summary>
        /// 用户答案
        /// </summary>
        /// <value>
        /// The user answers.
        /// </value>
        //public UserAnswer UserAnswers { get; set; }

        /// <summary>
        /// 是否显示用户答案
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is show user answer; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowUserAnswer { get; set; }

        /// <summary>
        /// 保存反序列化后的用户答案集合
        /// </summary>
        /// <value>
        /// The user result.
        /// </value>
        //public List<UserResult> UserResult { get; set; }

        /// <summary>
        /// 题目序号
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public string Index { get; set; }

        /// <summary>
        /// 收藏相同题目，用来区分答案选项
        /// </summary>
        /// <value>
        /// The index of the choice.
        /// </value>
        public int? ChoiceIndex { get; set; }

        /// <summary>
        /// 是否要生成pdf文件
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is create PDF; otherwise, <c>false</c>.
        /// </value>
        public bool IsCreatePdf { get; set; }

        public string ApplicationUrl { get; set; }

        public TiMuBuiltRequest(TiMu tiMu, string index = null, string applicationUrl = null, bool isAnalyticVisable = false, int? choiceIndex = null, bool isCreatePdf = false)
        {
            this.TiMu = tiMu;
            this.Index = index;
            this.IsAnalyticVisable = isAnalyticVisable;
            this.ChoiceIndex = choiceIndex;
            this.IsCreatePdf = isCreatePdf;
            this.ApplicationUrl = applicationUrl;
        }
    }
}
