using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Core;
using AF.Domain.Domain.Books;
using AF.Services.BookWork;
using AF.Domain.Domain.BookWork;
using AF.Domain.Domain.Knowledge;
using AF.Domain.Domain.TiMus;
using AF.Services.TiMus.TiMuCompose;

namespace AF.Web.Models.EntryExamination
{
    public class EntryExaminationModel
    {
        public EntryExaminationModel()
        {
            EntryExaminationListPagingModel = new EntryExaminationListPagingModel();
        }

        public EntryExaminationListPagingModel EntryExaminationListPagingModel { get; set; }

        /// <summary>
        /// 任务信息集合
        /// </summary>
        /// <value>
        /// The book task information list.
        /// </value>
        public IList<BookWorkTask> BookTaskInfoList { get; set; }

        /// <summary>
        /// 科目下拉集合
        /// </summary>
        /// <value>
        /// The subject list.
        /// </value>
        public IList<SelectListItem> SubjectList { get; set; }

        public int? SubjectId { get; set; }

        /// <summary>
        /// 题目对象
        /// </summary>
        /// <value>
        /// The ti mu.
        /// </value>
        [AllowHtml]
        public JsonTiMu Timu { get; set; }

        /// <summary>
        /// 题目集合
        /// </summary>
        /// <value>
        /// The ti mu list.
        /// </value>
        public IList<BookTiMu> TiMuList { get; set; }

        /// <summary>
        /// 题型
        /// </summary>
        /// <value>
        /// The type of the tm.
        /// </value>
        public IList<TiMuType> TmTypeList { get; set; }

        /// <summary>
        /// 题目类别
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public IList<string> Category { get; set; }

        /// <summary>
        /// 知识点输入筛选下拉框数据源
        /// </summary>
        /// <value>
        /// The knowledge json.
        /// </value>
        public RevertPage RevertPage { get; set; }

        /// <summary>
        /// 任务子项
        /// </summary>
        /// <value>
        /// The book work task item.
        /// </value>
        public BookWorkTaskItem BookWorkTaskItem { get; set; }

        /// <summary>
        /// 标识是创建题目还是编辑题目（create or edit）
        /// </summary>
        /// <value>
        /// The entry status.
        /// </value>
        public string EntryStatus { get; set; }

        /// <summary>
        /// 标识是否是回撤页面
        /// </summary>
        /// <value>
        /// The is revert.
        /// </value>
        public bool IsRevert { get; set; }

        public bool? IsEdit { get; set; }

        /// <summary>
        /// 保存上一次任务id，用于导航返回
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public int TaskId { get; set; }

        /// <summary>
        /// 章节集合
        /// </summary>
        /// <value>
        /// The book chapter.
        /// </value>
        public IList<BookChapter> BookChapterList { get; set; }

        /// <summary>
        /// 跳转编辑、录入题目页面使用
        /// </summary>
        /// <value>
        /// The entry HTML.
        /// </value>
        public string TimuId { get; set; }

        /// <summary>
        /// 任务包含的所有目录
        /// </summary>
        /// <value>
        /// All task chapter list.
        /// </value>
        public IList<BookChapter> AllTaskChapterList { get; set; }
        
        /// <summary>
        /// 题目排序
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int? Order { get; set; }

        /// <summary>
        /// 添加题目序号使用的（循环变量）
        /// </summary>
        /// <value>
        /// The cycles.
        /// </value>
        public int? Cycles { get; set; }

        /// <summary>
        /// 添加的子题目数量（循环变量）
        /// </summary>
        /// <value>
        /// The child cycle.
        /// </value>
        public int? ChildCycle { get; set; }

        /// <summary>
        /// 标定错误的枚举
        /// </summary>
        /// <value>
        /// The timu status.
        /// </value>
        public BookTiMu.TimuStatus? TimuStatus { get; set; }

        /// <summary>
        /// 回撤页面，判断是创建还是编辑子题目
        /// </summary>
        /// <value>
        /// The revert timu identifier.
        /// </value>
        public string RevertType { get; set; }
    }

    #region 题目Json对象
    /// <summary>
    /// 题目序列化对象
    /// </summary>
    public class JsonTiMu
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public new Guid Id { get; set; }

        /// <summary>
        /// BookTiMu排序字段，与TiMu无关
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int? Order { get; set; }

        /// <summary>
        /// 学科
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public int? SubjectId { get; set; }

        /// <summary>
        /// 题型
        /// </summary>
        /// <value>
        /// The ti mu type identifier.
        /// </value>
        public int? TiMuTypeId { get; set; }

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
        public int? Difficult { get; set; }

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
        //public virtual JsonTiMuAttributeExtend TiMuAttributeExtend { get; set; }

        /// <summary>
        /// 题目关联的学科对象
        /// </summary>
        public virtual JsonSubject Subject { get; set; }

        /// <summary>
        /// 题目关联的题目类型对象
        /// </summary>
        /// <value>
        /// The type of the ti mu.
        /// </value>
        //public virtual JsonTiMuType TiMuType { get; set; }

        /// <summary>
        /// 题目关联的能力类型对象
        /// </summary>
        /// <value>
        /// The ability.
        /// </value>
        //public virtual Ability Ability { get; set; }

        /// <summary>
        /// 题目关联的年级对象
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        //public virtual Grade Grade { get; set; }

        private ICollection<JsonInput> _input;
        /// <summary>
        /// 题目关联的答案集合
        /// </summary>
        public virtual ICollection<JsonInput> Inputs
        {
            get { return _input ?? (_input = new List<JsonInput>()); }
            set { _input = value; }
        }

        /*
        private ICollection<JsonTiMuKnowledge> _knowledge;
        /// <summary>
        /// 题目关联的题目知识点关系表集合
        /// </summary>
        public virtual ICollection<JsonTiMuKnowledge> Knowledges
        {
            get { return _knowledge ?? (_knowledge = new List<JsonTiMuKnowledge>()); }
            protected set { _knowledge = value; }
        }*/

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
    /// 答案序列化对象
    /// </summary>
    public class JsonInput
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

        private ICollection<JsonInputChoice> _inputChoice;
        /// <summary>
        /// 答案关联的选项集合
        /// </summary>
        public virtual ICollection<JsonInputChoice> InputChoice
        {
            get { return _inputChoice ?? (_inputChoice = new List<JsonInputChoice>()); }
            set { _inputChoice = value; }
        }
    }

    /// <summary>
    /// 选项序列化对象
    /// </summary>
    public class JsonInputChoice
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
    }

    public class JsonSubject
    {
        public int Id { get; set; }
        /// <summary>
        /// 科目名
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 科目编号
        /// </summary>
        /// <value>
        /// The system code.
        /// </value>
        public string SystemCode { get; set; }

        /// <summary>
        /// 学段
        /// </summary>
        /// <value>
        /// The degree identifier.
        /// </value>
        public int? DegreeId { get; set; }

        /// <summary>
        /// 父级科目id
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public int? ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int? Order { get; set; }

        /// <summary>
        /// 是否活动
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; set; }
    }

    public class JsonTiMuKnowledge
    {
        /// <summary>
        /// 题目id
        /// </summary>
        /// <value>
        /// The tm identifier.
        /// </value>
        public Guid TmId { get; set; }

        /// <summary>
        /// 知识点id
        /// </summary>
        /// <value>
        /// The knowledge identifier.
        /// </value>
        public int KnowledgeId { get; set; }

        /// <summary>
        /// 是否是主知识点
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is main; otherwise, <c>false</c>.
        /// </value>
        public bool IsMain { get; set; }
    }

    public class KnowledgePiont
    {
        public int Id { get; set; }

        /// <summary>
        /// 知识点id
        /// </summary>
        /// <value>
        /// The kpid.
        /// </value>
        public int Kpid { get; set; }

        /// <summary>
        /// 学科id
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public int SubjectId { get; set; }

        /// <summary>
        /// 知识点名称
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 父知识点id
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public int? ParentId { get; set; }
    }

    public class JsonTiMuType
    {
        public int Id { get; set; }
        /// <summary>
        /// 学科id
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public int SubjectId { get; set; }

        /// <summary>
        /// 题型编码
        /// </summary>
        /// <value>
        /// The name of the system.
        /// </value>
        public string SystemName { get; set; }

        /// <summary>
        /// 题型名称
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <value>
        /// The ordering.
        /// </value>
        public int Ordering { get; set; }
    }
    #endregion

    public class RevertPage
    {
        /// <summary>
        /// 题目id
        /// </summary>
        /// <value>
        /// The tmid.
        /// </value>
        public string Tmid { get; set; }

        /// <summary>
        /// 主知识点数据源
        /// </summary>
        /// <value>
        /// The main knowledge source.
        /// </value>
        public IList<KnowledgePiont> KnowledgeSource { get; set; }

        /// <summary>
        /// 相关知识点数据源
        /// </summary>
        /// <value>
        /// The minor knowledge source.
        /// </value>
        public IList<KnowledgePiont> MinorKnowledgeSource { get; set; }

        /// <summary>
        /// 本题目主知识点
        /// </summary>
        /// <value>
        /// The main ti mu knowledge.
        /// </value>
        public KnowledgePiont MainTiMuKnowledge { get; set; }

        /// <summary>
        /// 本题目相关知识点
        /// </summary>
        /// <value>
        /// The minor ti mu knowledge.
        /// </value>
        public IList<KnowledgePiont> MinorTiMuKnowledge { get; set; }

        /// <summary>
        /// 难度系数
        /// </summary>
        /// <value>
        /// The difference.
        /// </value>
        public decimal? Diff { get; set; }

        /// <summary>
        /// 视频id
        /// </summary>
        /// <value>
        /// The video identifier.
        /// </value>
        public string VideoId { get; set; }

        /// <summary>
        /// 讲解老师
        /// </summary>
        /// <value>
        /// The teacher.
        /// </value>
        public string Teacher { get; set; }

        /// <summary>
        /// 标记错误信息
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
    }

}