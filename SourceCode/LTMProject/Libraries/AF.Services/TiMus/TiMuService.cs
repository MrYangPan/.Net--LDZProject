using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;
using AF.Core.Data;
using AF.Core.Extensions;
using AF.Data.DbContext;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;
using AF.Domain.Domain.Knowledge;
using AF.Domain.Domain.TiMus;
using AF.Services.BookWork;
using AF.Services.Knowledge;
using AF.Services.TiMus.TiMuCompose;
using Newtonsoft.Json;

namespace AF.Services.TiMus
{
    /// <summary>
    /// 题目Service
    /// </summary>
    public class TiMuService : ITiMuService
    {
        #region 注入对象
        private readonly IDbContext _dbContext;
        private readonly IRepository<TiMu> _tiMuRepository;
        private readonly IRepository<TiMuKnowledge> _tiMuKnowledgeRepository;
        private readonly IRepository<BookWorkTask> _bookWorkTaskRepository;
        private readonly IRepository<BookWorkTaskItem> _bookWorkTaskItemRepository;
        private readonly IRepository<BookChapter> _bookChapterRepository;
        private readonly IRepository<BookTiMu> _bookTiMuRepository;
        private readonly IRepository<TiMuImages> _tiMuImagesRepository;
        private readonly IRepository<Input> _inputRepository;
        private readonly IRepository<InputChoice> _inputChoiceRepository;
        private readonly IRepository<ShushiTrack> _shushiTrackRepository;
        private readonly IRepository<TiMuType> _tiMuTypeRepository;
        private readonly IKnowledgeService _knowledgeService;
        #endregion

        public TiMuService(IDbContext dbContext, IRepository<TiMu> tiMuRepository, IRepository<BookWorkTask> bookWorkTaskRepository, IRepository<TiMuImages> tiMuImagesRepository, IRepository<Input> inputRepository, IRepository<InputChoice> inputChoiceRepository, IRepository<ShushiTrack> shushiTrackRepository, IRepository<TiMuType> tiMuTypeRepository, IRepository<BookChapter> bookChapterRepository, IRepository<BookTiMu> bookTiMuRepository, IRepository<TiMuKnowledge> tiMuKnowledgeRepository, IKnowledgeService knowledgeService, IRepository<BookWorkTaskItem> bookWorkTaskItemRepository)
        {
            _dbContext = dbContext;
            _tiMuRepository = tiMuRepository;
            _bookWorkTaskRepository = bookWorkTaskRepository;
            _tiMuImagesRepository = tiMuImagesRepository;
            _inputRepository = inputRepository;
            _inputChoiceRepository = inputChoiceRepository;
            _shushiTrackRepository = shushiTrackRepository;
            _tiMuTypeRepository = tiMuTypeRepository;
            _bookChapterRepository = bookChapterRepository;
            _bookTiMuRepository = bookTiMuRepository;
            _tiMuKnowledgeRepository = tiMuKnowledgeRepository;
            _knowledgeService = knowledgeService;
            _bookWorkTaskItemRepository = bookWorkTaskItemRepository;
        }

        /// <summary>
        /// 根据题目id获取题目对象
        /// </summary>
        /// <param name="tmid">The tmid.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public TiMu GeTiMuById(Guid tmid)
        {
            return _tiMuRepository.GetById(tmid);
        }

        /// <summary>
        /// 添加题目
        /// </summary>
        /// <param name="tiMu">The ti mu.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void InsertTiMu(TiMu tiMu)
        {
            if (tiMu == null)
                throw new ArgumentNullException(nameof(tiMu));
            _tiMuRepository.Insert(tiMu);
        }

        /// <summary>
        /// 根据章节获取题目分页
        /// </summary>
        /// <param name="chapterId">The chapter identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public IPagedList<TiMu> GetTiMuPagedList(int chapterId, int pageIndex = 0, int pageSize = Int32.MaxValue)
        {
            var chapter = _bookChapterRepository.GetById(chapterId);
            var timuIdList = _bookTiMuRepository.Table.Where(d => d.BookId == chapter.BookId && d.TaskItemId == chapter.TaskItemId).Select(d => d.Id);
            var timuList = _tiMuRepository.Table.Where(d => timuIdList.Contains(d.Id)).OrderBy(d => d.Id);
            return new PagedList<TiMu>(timuList, pageIndex, pageSize);
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="tiMuImages">The ti mu images.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void InsertImage(TiMuImages tiMuImages)
        {
            if (tiMuImages == null)
                throw new ArgumentNullException(nameof(tiMuImages));
            _tiMuImagesRepository.Insert(tiMuImages);
        }

        /// <summary>
        /// 添加答案
        /// </summary>
        /// <param name="input">The input.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void InsertInput(Input input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            _inputRepository.Insert(input);
        }

        /// <summary>
        /// 添加答案选项
        /// </summary>
        /// <param name="inputChoice">The input choice.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void InsertInputChoice(InputChoice inputChoice)
        {
            if (inputChoice == null)
                throw new ArgumentNullException(nameof(inputChoice));
            _inputChoiceRepository.Insert(inputChoice);
        }

        /// <summary>
        /// 根据guid获取竖式
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ShushiTrack GetShushiById(Guid guid)
        {
            return _shushiTrackRepository.GetById(guid);
        }

        /// <summary>
        /// 解析题目（选项、分析、解答、点评等）并且保存到相应的表
        /// </summary>
        /// <param name="timuStr">The timu string.</param>
        /// <param name="taskItemId">The task item identifier.</param>
        /// <param name="parentTmId">The parent tm identifier.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public string SaveTiMu(string timuStr, int? taskItemId, Guid? parentTmId, string property = null)
        {
            var question = "";
            string callbackTmid = string.Empty;
            try
            {
                //序列化前端页面题目对象
                var timuEntity = timuStr.Json2Obj<TimuJsonModel>();
                var timu = _tiMuRepository.GetById(timuEntity.TiMuId);
                callbackTmid = timuEntity.TiMuId.ToString();
                if (timu == null)
                {
                    timu = new TiMu() { Id = timuEntity.TiMuId, SubjectId = timuEntity.SubjectId,ParentId = parentTmId };
                    InsertTiMu(timu);
                    //把题目归属到对应的章节下面
                    var taskItem = _bookWorkTaskItemRepository.GetById(taskItemId);
                    var bookTimu = new BookTiMu()
                    {
                        TaskItemId = taskItem.Id,
                        Id = timu.Id,
                        BookId = taskItem.BookChapter.BookId,
                        Status = BookTiMu.TimuStatus.Valid,
                        Order = timuEntity.TimuBookOrder ?? 1
                    };
                    _bookTiMuRepository.Insert(bookTimu);
                    //保存一个空的答案数据到数据库，用于页面编辑前端js使用
                    var input = new Input()
                    {
                        Id = Guid.NewGuid(),
                        TmId = timu.Id,
                        InputCode = timu.Id + "_a",
                        BaseType = "text",
                        InputType = "",
                        InputAnswer = "",
                        Score = 0,
                        Order = 1,
                    };
                    callbackTmid = timu.Id.ToString();
                    _inputRepository.Insert(input);
                }
                var tiMuEntry = new TiMuEntry(timuEntity, timu);
                var result = tiMuEntry.TiMuFormat();
                //更新题目
                _tiMuRepository.Update(result.TiMu);
                //如果有答案则更新，没有就添加
                foreach (Input d in result.InputList)
                {
                    var input = _inputRepository.GetById(d.Id);
                    if (input != null)
                        _inputRepository.Update(d);
                    else
                        _inputRepository.Insert(d);
                }
                //如果有选项则更行，没有就添加
                foreach (InputChoice d in result.InputChoiceList)
                {
                    var choice = _inputChoiceRepository.GetById(d.Id);
                    if (choice != null)
                        _inputChoiceRepository.Update(d);
                    else
                        _inputChoiceRepository.Insert(d);
                }
                //删除答案
                if (result.DeleteInputs != null && result.DeleteInputs.Any())
                {
                    _inputRepository.Delete(result.DeleteInputs);
                }
                //更新booktimu相關信息
                var booktimu = timu.BookTiMu;
                if (booktimu!=null)
                {
                    booktimu.LargeNumber = timuEntity.LargeNumber;
                    booktimu.SmallNumber = timuEntity.SmallNumber;
                    booktimu.PageNumber = timuEntity.PageNumber;
                    _bookTiMuRepository.Update(booktimu);
                }
                question = "保存成功!";
                if (property != null)
                {
                    question = SaveTiMuMark(property);
                }
            }
            catch (Exception e)
            {
                question = "保存失败:" + e.Message;
            }
            var obj = new { info = question, tmid = callbackTmid};
            return obj.Obj2Json();
        }

        /// <summary>
        /// 保存题目标定的属性
        /// </summary>
        /// <param name="property">The property.</param>
        public string SaveTiMuMark(string property = null)
        {
            var result = "";
            try
            {
                //保存题目相关属性信息
                if (property != null)
                {
                    //序列化属性对象
                    var revert = property.Json2Obj<ReverteJson>();
                    var timu = _tiMuRepository.GetById(revert.Tmid);
                    var timuKnows = _tiMuKnowledgeRepository.Table;
                    //保存主知识点
                    var queryM = timuKnows.FirstOrDefault(d => d.TmId == timu.Id && d.IsMain);
                    if (queryM == null && revert.MainId != null)
                    {
                        var knowledge = new TiMuKnowledge()
                        {
                            TmId = timu.Id,
                            IsMain = true,
                            KnowledgeId = revert.MainId.Value
                        };
                        _tiMuKnowledgeRepository.Insert(knowledge);
                    }
                    else if (revert.MainId != null)
                    {
                        queryM.KnowledgeId = (int)revert.MainId;
                        _tiMuKnowledgeRepository.Update(queryM);
                    }
                    if (!string.IsNullOrEmpty(revert.MinorIds))
                    {
                        //保存相关知识点
                        var minorids = revert.MinorIds.Split(',');
                        for (int i = 0; i < minorids.Length; i++)
                        {
                            var minor = Convert.ToInt32(minorids[i]);
                            var querym = timuKnows.Where(d => d.TmId == timu.Id && d.KnowledgeId == minor && !d.IsMain);
                            if (!querym.Any())
                            {
                                var knowledge = new TiMuKnowledge()
                                {
                                    TmId = timu.Id,
                                    IsMain = false,
                                    KnowledgeId = Int32.Parse(minorids[i])
                                };
                                _tiMuKnowledgeRepository.Insert(knowledge);
                            }
                        }
                    }
                    //保存难度等级
                    timu.Difficult = revert.Diff;
                    //保存微课视频id
                    //timu.VideoCode = revert.VideoCode;
                    _tiMuRepository.Update(timu);
                    //保存讲解老师...
                    
                    //清空错误信息
                    var bookTimu= timu.BookTiMu;
                    if (!string.IsNullOrWhiteSpace(bookTimu?.ErrorInfo))
                    {
                        bookTimu.ErrorInfo = "";
                        _bookTiMuRepository.Update(bookTimu);
                    }
                }
                result = "保存成功";
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 根据科目id获取所有题型
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<TiMuType> GetAllTypes(int subjectId)
        {
            var list = _tiMuTypeRepository.Table.Where(d => d.SubjectId == subjectId).ToList();
            return list;
        }

        /// <summary>
        /// 根据选项id，删除选项
        /// </summary>
        /// <param name="choiceId">The choice identifier.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void DeleteChoice(Guid? choiceId)
        {
            var inputChoice = _inputChoiceRepository.GetById(choiceId);
            if (inputChoice != null)
                //删除最后一个选项
                _inputChoiceRepository.Delete(inputChoice);
        }

        /// <summary>
        /// 录题页面编辑使用
        /// </summary>
        /// <param name="tmid">The tmid.</param>
        /// <returns></returns>
        public TiMu EntryGeTiMuFormatById(Guid tmid)
        {
            var timu = _tiMuRepository.GetById(tmid);
            TiMuEntry.EntryGeTiMuFormatById(timu);
            return timu;
        }

        /// <summary>
        /// 删除题目
        /// </summary>
        /// <param name="models">The models.</param>
        public void DeleteTiMu(params TiMu[] models)
        {
            if (models == null) throw new ArgumentNullException(nameof(models));
            _tiMuRepository.Delete(models);
        }

        /// <summary>
        /// Updates the ti mu.
        /// </summary>
        /// <param name="tiMu">The ti mu.</param>
        public void UpdateTiMu(TiMu tiMu)
        {
            _tiMuRepository.Update(tiMu);
        }

        #region BookTiMu        
        /// <summary>
        /// 根据书本id获取BookTiMu
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <returns></returns>
        public IList<BookTiMu> GetBookTiMuList(int bookId)
        {
            return _bookTiMuRepository.Table.Where(t => t.BookId == bookId).ToList();
        }

        /// <summary>
        /// 删除BookTiMu
        /// </summary>
        /// <param name="model">The model.</param>
        public void DeleteBookTiMu(params BookTiMu[] models)
        {
            if (models == null) throw new ArgumentNullException(nameof(models));
            _bookTiMuRepository.Delete(models);
        }

        #endregion
    }
}
