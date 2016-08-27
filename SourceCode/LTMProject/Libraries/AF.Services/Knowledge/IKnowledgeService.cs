using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Knowledge;

namespace AF.Services.Knowledge
{
    public interface IKnowledgeService
    {
        /// <summary>
        /// 根据题目id、科目id获取主要知识点对象
        /// </summary>
        /// <param name="tmId">The tm identifier.</param>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        KnowledgePiont GetMainKnowledgeByTiMu(Guid tmId, int? subjectId = null);
        /// <summary>
        /// 根据科目类型，获取所有的知识点
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        IList<KnowledgePiont> GetKnowledge(int subjectId, int? parentId = null);

        /// <summary>
        /// 获取所有的子孙知识点id集合
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        IList<KnowledgePiont> GetChildPoints(int subjectId, int? parentId);

        /// <summary>
        /// 取题目知识点
        /// </summary>
        /// <param name="tmId">The tm identifier.</param>
        /// <returns></returns>
        TiMuKnowledge GetTiMuKnowledgeByTiMu(Guid tmId);

        /// <summary>
        /// 根据题目id、科目类型，获取相关知识点集合
        /// </summary>
        /// <param name="tmId">The tm identifier.</param>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        IList<KnowledgePiont> GetMinorTiMuKnowledgeByTiMu(Guid tmId, int? subjectId = null);

        /// <summary>
        /// 根据知识点id、题目id删除题目知识点
        /// </summary>
        /// <param name="knowledgeId">The knowledge identifier.</param>
        /// <param name="tmid">The tmid.</param>
        void DeleteTiMuKnowledgeById(int knowledgeId, Guid tmid);

        /// <summary>
        /// 获取知识点
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        KnowledgePiont GetKnowledgeById(int id);

        /// <summary>
        /// 添加知识点
        /// </summary>
        /// <param name="model">The model.</param>
        void InsertKnowledge(KnowledgePiont model);

        /// <summary>
        /// 修改知识点
        /// </summary>
        /// <param name="model">The model.</param>
        void UpdateKnowledge(KnowledgePiont model);

        /// <summary>
        /// 删除知识点，同事删除知识点题目关联表
        /// </summary>
        /// <param name="knowledgeId">The knowledge identifier.</param>
        void DeleteKnowledge(int knowledgeId);
    }
}
