using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;
using AF.Core.Data;
using AF.Data.DbContext;
using AF.Domain.Domain.Knowledge;
using AF.Domain.Domain.TiMus;


namespace AF.Services.Knowledge
{
    public class KnowledgeService : IKnowledgeService
    {
        #region Fields
        private readonly IDbContext _dbContext;
        private readonly IRepository<Domain.Domain.Knowledge.KnowledgePiont> _knowledgeRepository;
        private readonly IRepository<TiMuKnowledge> _tiMuKnowledgeRepository;

        private readonly IRepository<TiMu> _tiMuRepository;
        #endregion
        #region Ctor
        public KnowledgeService(
            IDbContext dbContext,
            IRepository<Domain.Domain.Knowledge.KnowledgePiont> knowledgeRepository,
            IRepository<TiMuKnowledge> tiMuKnowledgeRepository,

            IRepository<TiMu> tiMuRepository
            )
        {
            _dbContext = dbContext;
            _knowledgeRepository = knowledgeRepository;
            _tiMuKnowledgeRepository = tiMuKnowledgeRepository;

            _tiMuRepository = tiMuRepository;
        }
        #endregion

        #region 共用
        /// <summary>
        /// 递归获取子孙知识点的ID集合
        /// </summary>
        /// <param name="knowledgeList">The knowledge list.</param>
        /// <param name="knowledgeSorce"></param>
        /// <param name="kpidList">The kpid list.</param>
        private void GetChildKnowledgeRecurrent(IList<Domain.Domain.Knowledge.KnowledgePiont> knowledgeList, IList<Domain.Domain.Knowledge.KnowledgePiont> knowledgeSorce, List<KnowledgePiont> kpidList)
        {
            foreach (var knowledgePiont in knowledgeList)
            {
                var tempList = knowledgeSorce.Where(p => p.ParentId == knowledgePiont.Id).ToList();
                kpidList.Add(knowledgePiont);
                //递归调用
                GetChildKnowledgeRecurrent(tempList, knowledgeSorce, kpidList);
            }
        }
        #endregion

        /// <summary>
        /// 取题目知识点
        /// </summary>
        /// <param name="tmId">The tm identifier.</param>
        /// <returns></returns>
        public TiMuKnowledge GetTiMuKnowledgeByTiMu(Guid tmId)
        {
            return _tiMuKnowledgeRepository.Table.FirstOrDefault(t => t.TmId == tmId && t.IsMain);
        }

        /// <summary>
        /// 根据题目id、科目类型，获取相关知识点集合
        /// </summary>
        /// <param name="tmId">The tm identifier.</param>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        public IList<KnowledgePiont> GetMinorTiMuKnowledgeByTiMu(Guid tmId, int? subjectId = null)
        {
            var query = from a in _tiMuKnowledgeRepository.Table
                        join b in _knowledgeRepository.Table on a.KnowledgeId equals b.Id
                        where a.TmId == tmId && !a.IsMain
                        select b;

            if (subjectId.HasValue)
            {
                query = query.Where(p => p.SubjectId == subjectId);
            }
            return query.ToList();
        }

        /// <summary>
        /// 根据知识点id、题目id删除题目知识点
        /// </summary>
        /// <param name="knowledgeId">The knowledge identifier.</param>
        /// <param name="tmid">The tmid.</param>
        public void DeleteTiMuKnowledgeById(int knowledgeId, Guid tmid)
        {
            var entry = _tiMuKnowledgeRepository.Table.Where(d => d.KnowledgeId == knowledgeId && d.TmId == tmid);
            _tiMuKnowledgeRepository.Delete(entry);
        }

        /// <summary>
        /// 根据题目id、科目id获取主要知识点对象
        /// </summary>
        /// <param name="tmId">The tm identifier.</param>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        public KnowledgePiont GetMainKnowledgeByTiMu(Guid tmId, int? subjectId = null)
        {
            var query = from a in _tiMuKnowledgeRepository.Table
                        join b in _knowledgeRepository.Table on a.KnowledgeId equals b.Id
                        where a.TmId == tmId && a.IsMain
                        select b;

            if (subjectId.HasValue)
            {
                query = query.Where(p => p.SubjectId == subjectId);
            }
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 根据科目类型，获取所有的知识点
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        public IList<Domain.Domain.Knowledge.KnowledgePiont> GetKnowledge(int subjectId, int? parentId = null)
        {
            var knowledgeList = _knowledgeRepository.Table.Where(p => p.SubjectId == subjectId);
            if (parentId.HasValue)
                knowledgeList = knowledgeList.Where(p => p.ParentId == parentId);
            return knowledgeList.ToList();
        }

        /// <summary>
        /// 获取所有的子孙知识点id集合
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="kpid">The kpid.</param>
        /// <returns></returns>
        public IList<KnowledgePiont> GetChildPoints(int subjectId, int? kpid)
        {
            var knowledList = GetKnowledge(subjectId);
            //获取当前结点的id
            var own = knowledList.ToList();
            var kpids = new List<KnowledgePiont>();
            if (kpid != null)
            {
                own.Where(d => d.Id == kpid);
                //添加当前节点到集合
                own.ForEach(d => { kpids.Add(d); });
            }
            //获取子节点
            var knowledges = knowledList.Where(d => d.ParentId == kpid).ToList();
            GetChildKnowledgeRecurrent(knowledges, knowledList, kpids);
            return kpids;
        }

        /// <summary>
        /// 获取知识点
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public KnowledgePiont GetKnowledgeById(int id)
        {
            return _knowledgeRepository.Table.FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// 添加知识点
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void InsertKnowledge(KnowledgePiont model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            _knowledgeRepository.Insert(model);
        }

        /// <summary>
        /// 修改知识点
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void UpdateKnowledge(KnowledgePiont model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            _knowledgeRepository.Update(model);
        }

        /// <summary>
        /// 删除知识点，同事删除知识点题目关联表
        /// </summary>
        /// <param name="knowledgeId">The knowledge identifier.</param>
        public void DeleteKnowledge(int knowledgeId)
        {
            //删除知识点，同事删除知识点题目关联表
            var knowledge = _knowledgeRepository.GetById(knowledgeId);

            if (knowledge == null)
                throw new ArgumentNullException(nameof(knowledge));

            var timuKnowledges = _tiMuKnowledgeRepository.Table.Where(t => t.KnowledgeId == knowledgeId);
            if (timuKnowledges.Any())
                _tiMuKnowledgeRepository.Delete(timuKnowledges);

            _knowledgeRepository.Delete(knowledge);
        }

    }
}
