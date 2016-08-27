using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AF.Core.Extensions;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.TiMus;
using AF.Services.Books;
using AF.Services.BookWork;
using AF.Services.Question;
using AF.Services.TiMus;
using AF.Services.TiMus.TiMuCompose;
using AF.Web.Models.EntryExamination;
using AF.Web.Models.MarkProperty;
using Newtonsoft.Json;
using EntryTiMuModel = AF.Web.Models.EntryExamination.EntryTiMuModel;
using AF.Domain.Domain.BookWork;
using AF.Domain.Domain.Customers;
using AF.Domain.Infrastructure;
using AF.Services.Knowledge;
namespace AF.Web.Controllers
{
    /// <summary>
    /// 录入试题Controller
    /// </summary>
    public class EntryExaminationController : BasePublicController
    {
        #region 初始化接口
        private readonly IWorkContext _workContext;
        private readonly ITiMuService _tiMuService;
        private readonly IBookWorkTaskService _bookWorkTaskService;
        private readonly ISubjectService _subjectService;
        private readonly IBookChapterService _bookChapterService;
        private readonly IQuestionService _questionService;
        private readonly IBookWorkTaskItemService _bookWorkTaskItemService;
        private readonly IKnowledgeService _knowledgeService;
        public EntryExaminationController(ITiMuService tiMuService, IBookWorkTaskService bookWorkTaskService, ISubjectService subjectService, IBookChapterService bookChapterService, IQuestionService questionService, IWorkContext workContext, IBookWorkTaskItemService bookWorkTaskItemService, IKnowledgeService knowledgeService)
        {
            _tiMuService = tiMuService;
            _bookWorkTaskService = bookWorkTaskService;
            _subjectService = subjectService;
            _bookChapterService = bookChapterService;
            _questionService = questionService;
            _workContext = workContext;
            _bookWorkTaskItemService = bookWorkTaskItemService;
            _knowledgeService = knowledgeService;
        }

        #endregion

        #region

        private void PrepareTopic(EntryExaminationModel model, TiMu timu)
        {
            //根据科目id获取所以子孙知识点集合
            var knowledegs = _knowledgeService.GetChildPoints(timu.SubjectId, null);
            var knowledgeSource = new List<KnowledgePiont>();
            knowledegs.ForEach(d =>
            {
                knowledgeSource.Add(new KnowledgePiont()
                {
                    Id = d.Id,
                    Kpid = d.Id,
                    Name = d.Name,
                    ParentId = d.ParentId,
                    SubjectId = d.SubjectId
                });
            });
            var revertPage = new RevertPage();
            revertPage.Tmid = timu.Id.ToString();
            revertPage.KnowledgeSource = knowledgeSource;
            //获取本题目主知识点
            var maink = _knowledgeService.GetMainKnowledgeByTiMu(timu.Id, timu.SubjectId);
            //获取本题目相关知识点
            var minors = _knowledgeService.GetMinorTiMuKnowledgeByTiMu(timu.Id, timu.SubjectId);
            var minorList = new List<KnowledgePiont>();
            revertPage.Diff = timu.Difficult;
            revertPage.VideoId = timu.VideoCode;
            revertPage.Teacher = "";
            revertPage.ErrorMessage = timu.BookTiMu.ErrorInfo;
            if (maink != null)
            {
                revertPage.MainTiMuKnowledge = new KnowledgePiont()
                {
                    Id = maink.Id,
                    Kpid = maink.Id,
                    Name = maink.Name,
                    ParentId = maink.ParentId,
                    SubjectId = maink.SubjectId
                };
            }
            else
            {
                revertPage.MainTiMuKnowledge = null;
            }
            minors.ForEach(d =>
            {
                minorList.Add(new KnowledgePiont()
                {
                    Id = d.Id,
                    Kpid = d.Id,
                    Name = d.Name,
                    ParentId = d.ParentId,
                    SubjectId = d.SubjectId
                });
            });
            revertPage.MinorTiMuKnowledge = minorList;
            model.RevertPage = revertPage;
        }

        private void TiMuConvert(EntryExaminationModel model, TiMu timu)
        {
            var inputs = new List<JsonInput>();
            var choices = new List<JsonInputChoice>();
            foreach (Input input in timu.Inputs.OrderBy(d => d.Order))
            {
                //选项
                foreach (InputChoice choice in input.InputChoice.OrderBy(d => d.Order))
                {
                    choices.Add(new JsonInputChoice()
                    {
                        Id = choice.Id,
                        InputId = choice.InputId,
                        ChoiceContent = choice.ChoiceContent,
                        Score = choice.Score,
                        Order = choice.Order
                    });
                }
                //答案
                inputs.Add(new JsonInput()
                {
                    Id = input.Id,
                    TmId = input.TmId,
                    InputCode = input.InputCode,
                    BaseType = input.BaseType,
                    InputType = input.InputType,
                    Score = input.Score,
                    Order = input.Order,
                    InputAnswer = input.InputAnswer,
                    InputChoice = choices
                });
            }
            model.Timu = new JsonTiMu()
            {
                Id = timu.Id,
                SubjectId = timu.SubjectId,
                TiMuTypeId = timu.TiMuTypeId,
                Trunk = timu.Trunk,
                Analysis = timu.Analysis,
                Answer = timu.Answer,
                Comment = timu.Comment,
                Difficult = timu.Difficult,
                Discriminate = timu.Discriminate,
                StandardTime = timu.StandardTime,
                StanderdScroe = timu.StanderdScroe,
                AbilityId = timu.AbilityId,
                GradeId = timu.GradeId,
                Year = timu.Year,
                Soure = timu.Soure,
                QualityId = timu.QualityId,
                Price = timu.Price,
                Inputs = inputs,
                LargeNumber = timu.BookTiMu.LargeNumber,
                SmallNumber = timu.BookTiMu.SmallNumber,
                PageNumber = timu.BookTiMu.PageNumber
            };
            if (timu.Subject != null)
            {
                model.Timu.Subject = new JsonSubject()
                {
                    Id = timu.Subject.Id,
                    Name = timu.Subject.Name,
                    SystemCode = timu.Subject.SystemCode,
                    DegreeId = timu.Subject.DegreeId,
                    ParentId = timu.Subject.ParentId,
                    Order = timu.Subject.Order,
                    Active = timu.Subject.Active
                };
            }
        }

        private void TiMuCreateEmpty(EntryExaminationModel model, Subject subject, int? order)
        {
            var inputs = new List<JsonInput>();
            var choices = new List<JsonInputChoice>();
            var timuId = Guid.NewGuid();
            //答案
            inputs.Add(new JsonInput()
            {
                Id = Guid.NewGuid(),
                TmId = timuId,
                InputCode = timuId.ToString(),
                BaseType = "text",
                InputType = "long-input",
                Order = 1,
                InputAnswer = string.Empty,
                InputChoice = choices
            });
            model.Timu = new JsonTiMu()
            {
                Id = timuId,
                SubjectId = subject.Id,
                TiMuTypeId = subject.TiMuTypes.First().Id,
                Trunk = string.Empty,
                Analysis = string.Empty,
                Answer = string.Empty,
                Comment = string.Empty,
                Order = order,
                Subject = new JsonSubject()
                {
                    Id = subject.Id,
                    Name = subject.Name,
                    SystemCode = subject.SystemCode,
                    DegreeId = subject.DegreeId,
                    ParentId = subject.ParentId,
                    Order = subject.Order,
                    Active = subject.Active
                },
                Inputs = inputs
            };
        }

        #endregion

        // GET: EntryExamination
        public ActionResult Index(EntryExaminationModel model, EntryExaminationListPagingModel command)
        {
            if (command.PageNumber <= 0)
                command.PageNumber = 1;
            if (null == model)
                model = new EntryExaminationModel();
            var subject = _subjectService.GetSubjectList();
            var selectList = new List<SelectListItem>() { new SelectListItem() { Text = "请选择", Value = string.Empty } };
            subject.ForEach(d =>
            {
                selectList.Add(new SelectListItem() { Text = d.Name, Value = d.Id.ToString() });
            });
            model.SubjectList = selectList;
            int? userid = null;
            if (!_workContext.CurrentCustomer.IsAdmin())
            {
                userid = _workContext.CurrentCustomer.Id;
            }
            var bookTasks = _bookWorkTaskService.GetAllTasks(userId: userid, taskStatus: TaskStatus.Entry, subjectId: model.SubjectId, pageIndex: command.PageNumber - 1,
                pageSize: command.PageSize);
            model.BookTaskInfoList = bookTasks;
            model.EntryExaminationListPagingModel.LoadPagedList(bookTasks);
            return View(model);
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitResult(int taskId, TaskStatus status)
        {
            var booktask = _bookWorkTaskService.GetBookWorkTask(taskId);
            booktask.Status = status;
            _bookWorkTaskService.SubmitTask(booktask);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Checks the task is done.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        public JsonResult CheckTaskIsDone(int taskId)
        {
            //var task = _bookWorkTaskService.GetBookWorkTask(taskId);
            //var taskAllTimuIdList = _bookWorkTaskService.GetTaskBookTiMus(taskId).Select(p => p.Id).ToList();
            object result = new { IsDone = true, Text = "提交后无法再进行修改，确认提交吗?" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出 PPT文件
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportPpt()
        {


            return Json("");
        }

        /// <summary>
        /// 任务详情
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskDetail(int taskId, bool? revert)
        {
            var model = new EntryExaminationModel();
            var task = _bookWorkTaskService.GetBookWorkTask(taskId);
            model.AllTaskChapterList = task.BookWorkTaskItems.Select(d => d.BookChapter).ToList();
            var bookChapterList = new List<BookChapter>();
            foreach (var titem in task.BookWorkTaskItems)
            {
                var itemchapter = titem.BookChapter;
                if (itemchapter != null)
                {
                    while (itemchapter.BookChapterParent != null)
                    {
                        itemchapter = itemchapter.BookChapterParent;
                    }
                    if (!bookChapterList.Contains(itemchapter))
                    {
                        bookChapterList.Add(itemchapter);
                    }
                }
            }
            model.BookChapterList = bookChapterList.OrderBy(p => p.Id).ToList();
            model.TaskId = taskId;
            model.IsRevert = revert.HasValue ? revert.Value : false;
            return View(model);
        }

        /// <summary>
        /// 录入试题（跳转编辑或者创建页面）
        /// </summary>
        /// <param name="viewmodel">The viewmodel.</param>
        /// <returns></returns>
        public ActionResult EntryTopic(EntryTopicModel viewmodel)
        {
            var model = new EntryExaminationModel();
            var taskitem = _bookWorkTaskItemService.GetBookWorkTaskItemsById(viewmodel.taskItemId);
            //获取当前章节的父题目
            model.TiMuList = _bookWorkTaskItemService.GetBookTiMus(viewmodel.taskItemId).Where(d=>d.Timu.ParentId==null).ToList();
            model.BookWorkTaskItem = taskitem;
            model.TaskId = viewmodel.taskId;
            model.IsRevert = viewmodel.revert.HasValue ? viewmodel.revert.Value : false;
            model.TimuId = viewmodel.timuId;
            model.IsEdit = viewmodel.isEdit;
            model.Order = viewmodel.order;
            if ((viewmodel.order != null && viewmodel.cycles != null) || (viewmodel.order == null && viewmodel.cycles != null))
            {
                model.Cycles = viewmodel.cycles;
            }
            model.TimuStatus = viewmodel.timuStatus;
            model.ChildCycle = viewmodel.childCycle;
            model.RevertType = viewmodel.revertType;
            return View(model);
        }

        /// <summary>
        /// 题目编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult TopicEdit(Guid tmid, int? order, int? cycles)
        {
            var model = new EntryExaminationModel();
            var timu = _tiMuService.EntryGeTiMuFormatById(tmid);
            TiMuConvert(model, timu);
            PrepareTopic(model, timu);
            model.TmTypeList = _tiMuService.GetAllTypes(timu.SubjectId);
            model.BookWorkTaskItem = _bookWorkTaskItemService.GetBookTiMuById(tmid).TaskItem;
            model.Order = order;
            model.Cycles = cycles;
            return PartialView(model);
        }

        /// <summary>
        /// 创建题目
        /// </summary>
        /// <param name="taskItemId">The task item identifier.</param>
        /// <param name="tmid">The tmid.</param>
        /// <param name="order">The order.</param>
        /// <param name="cycles">The cycles.</param>
        /// <returns></returns>
        public ActionResult TopicCreate(int taskItemId, string tmid, int? order)
        {
            if (tmid != null)
            {
                return RedirectToAction("TopicEdit", new { tmid = tmid, order = order });
            }
            else
            {
                var model = new EntryExaminationModel();
                var taskitem = _bookWorkTaskItemService.GetBookWorkTaskItemsById(taskItemId);
                model.BookWorkTaskItem = taskitem;
                var subject = taskitem.BookWorkTask.Book.Subject;
                TiMuCreateEmpty(model, subject, order);
                model.TmTypeList = subject.TiMuTypes.ToList();
                model.TimuId = tmid;
                model.Order = order;
                return PartialView(model);
            }
        }

        /// <summary>
        /// 属性标定
        /// </summary>
        /// <param name="tmid">The tmid.</param>
        /// <param name="timuStatus">The timu status.</param>
        /// <returns></returns>
        public ActionResult TopicMark(Guid tmid, BookTiMu.TimuStatus? timuStatus)
        {
            var model = new EntryExaminationModel();
            var timu = _tiMuService.EntryGeTiMuFormatById(tmid);
            TiMuConvert(model, timu);
            PrepareTopic(model, timu);
            model.TmTypeList = _tiMuService.GetAllTypes(timu.SubjectId);
            model.TimuStatus = timuStatus;
            return PartialView(model);
        }

        /// <summary>
        /// 被撤回任务的录题、属性标定页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RevertMark(Guid tmid, BookTiMu.TimuStatus? timuStatus)
        {
            var model = new EntryExaminationModel();
            var timu = _tiMuService.EntryGeTiMuFormatById(tmid);
            TiMuConvert(model, timu);
            PrepareTopic(model, timu);
            model.TmTypeList = _tiMuService.GetAllTypes(timu.SubjectId);
            model.TimuId = tmid.ToString();
            model.TimuStatus = timuStatus;
            return PartialView(model);
        }

        /// <summary>
        /// 删除选项
        /// </summary>
        /// <param name="choiceId">The choice identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteChoice(Guid? choiceId)
        {
            var result = "";
            try
            {
                if (choiceId != null)
                    _tiMuService.DeleteChoice(choiceId);
                result = "删除成功!";
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 题目录入保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveTopic(string timu, int? taskItemId, Guid? parentTmId, string property = null)
        {
            //错误提示
            var result = _tiMuService.SaveTiMu(timu, taskItemId, parentTmId, property);
            return Json(result);
        }

        /// <summary>
        /// 保存题目标定属性
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveTiMuMark(string property)
        {
            var result = _tiMuService.SaveTiMuMark(property);
            return Json(new { result = result });
        }

        /// <summary>
        /// 试题预览
        /// </summary>
        /// <returns></returns>
        public ActionResult PreviewTopic(EntryExaminationListPagingModel command, int taskItemId)
        {
            if (command.PageNumber <= 0)
                command.PageNumber = 1;
            var model = new EntryTiMuModel();
            var booktimus = _bookWorkTaskItemService.GetBookTiMus(taskItemId);
            for (var i = 0; i < booktimus.Count; i++)
            {
                var tbbr = new TiMuBuiltRequest(tiMu: booktimus[i].Timu, index: (i + 1).ToString(), applicationUrl: "http://" + Request.Url.Authority);
                var tmbuder = new TiMuBuild(tbbr);
                model.TiMulIsList.Add(tmbuder.AssemblyTimu());
            }
            var taskItem = _bookWorkTaskItemService.GetBookWorkTaskItemsById(taskItemId);
            model.BookWorkTask = taskItem.BookWorkTask;
            model.BookChapterList = _bookChapterService.GetParentListById(taskItem.ChapterId);
            return View(model);
        }

        /// <summary>
        /// 答案解析
        /// </summary>
        /// <returns></returns>
        public ActionResult AnswerAnalysis(string analyseName, Guid tmid)
        {
            var timu = _tiMuService.GeTiMuById(tmid);
            var tbbr = new TiMuBuiltRequest(timu);
            var tmbuder = new TiMuBuild(tbbr);
            var tiMuAnalyse = tmbuder.PingTiMuAnalyse(analyseName);

            return
                Json(
                    new
                    {
                        MuAnalyse = tiMuAnalyse[0],
                        InputType = tiMuAnalyse[1],
                        Answer = tiMuAnalyse[2],
                        InputCode = tiMuAnalyse[3]
                    }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 被撤回任务
        /// </summary>
        /// <returns></returns>
        public ActionResult Revocation(EntryExaminationModel model, EntryExaminationListPagingModel command)
        {
            if (command.PageNumber <= 0)
                command.PageNumber = 1;
            if (null == model)
                model = new EntryExaminationModel();
            int? userid = null;
            if (!_workContext.CurrentCustomer.IsAdmin())
            {
                userid = _workContext.CurrentCustomer.Id;
            }
            var bookTasks = _bookWorkTaskService.GetAllTasks(userId: userid, taskStatus: TaskStatus.Revert, pageIndex: command.PageNumber - 1,
                pageSize: command.PageSize);
            model.BookTaskInfoList = bookTasks;
            model.EntryExaminationListPagingModel.LoadPagedList(bookTasks);
            return View(model);
        }

        /// <summary>
        /// 删除知识点
        /// </summary>
        /// <param name="knowledgeId">The knowledge identifier.</param>
        /// <param name="tmid">The tmid.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteKnowledgeById(int knowledgeId, Guid tmid)
        {
            var result = "";
            try
            {
                _knowledgeService.DeleteTiMuKnowledgeById(knowledgeId, tmid);
                result = "删除成功";
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return Json(result);
        }

        public ActionResult SaveTimuVideo(Guid timuId,string videoCode)
        {
            var result = "";
            try
            {
                var timu = _tiMuService.GeTiMuById(timuId);
                timu.VideoCode = videoCode;
                _tiMuService.UpdateTiMu(timu);
                result = "上传成功!";
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return Json(result);
        }

        #region 公式编辑器

        public ActionResult TiMuEdit(string tmid = "16CC6275-7B25-4D1B-AE71-F6455272B58B")
        {
            var model = new EntryTiMuModel() { Tmid = tmid };
            return View(model);
        }

        /// <summary>
        /// Uploads the image.
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadImage(HttpPostedFileBase file, Guid tmid,string subjcode)
        {
            var timu = _tiMuService.GeTiMuById(tmid);
            var info = new ImageResult();
            if (file != null && file.ContentLength > 0)
            {
                string fix = Path.GetExtension(file.FileName)?.ToLower();
                if (fix != ".bmp" && fix != ".gif" && fix != ".jpg" && fix != ".png")
                {
                    info.Error = "请上传bmp、gif、jpg、png格式的文件。";
                }
                else
                {
                    var tmImageName = Guid.NewGuid();
                    string reName = tmImageName + fix;
                    //根据科目创建文件夹
                    string path = "/Content/Upload/" + subjcode + "/";
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(file.InputStream))
                        {
                            var tiMuImages = new TiMuImages
                            {
                                Tmid = tmid.ToString(),
                                Status = false,
                                Name = reName,
                                ImgPath = path + reName,
                                UploadDate = DateTime.Now,
                                CustomerId = 0//_workContext.CurrentCustomer.Id
                            };
                            _tiMuService.InsertImage(tiMuImages);
                            info.ImagePath = "[img=" + img.Width + "," + img.Height + "]" + reName + "[/img]";
                            file.SaveAs(Server.MapPath(path + reName));
                        }
                    }
                    catch (Exception e)
                    {
                        info.Error = e.Message;
                    }
                }
            }
            return View(info);
        }

        #region 术式

        /// <summary>
        /// Gets the track.
        /// </summary>
        /// <param name="tmid">The tmid.</param>
        /// <param name="trackId">The track identifier.</param>
        /// <param name="trackContent">Content of the track.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public string GetTrack(string tmid, string trackId, string trackContent)
        {
            var track = _questionService.GetTmShushiTrackById(trackId);
            return track != null ? track.Track : String.Empty;
        }

        /// <summary>
        /// Updates the track.
        /// </summary>
        /// <param name="tmid">The tmid.</param>
        /// <param name="trackId">The track identifier.</param>
        /// <param name="trackContent">Content of the track.</param>
        /// <returns>System.String.</returns>
        public string UpdateTrack(string tmid, string trackId, string trackContent)
        {
            var track = _questionService.UpdateTmShushiTrack(tmid, trackId, trackContent);
            return track.Id.ToString();
        }
        #endregion

        #endregion
    }
}