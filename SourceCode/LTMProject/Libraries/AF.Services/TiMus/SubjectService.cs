using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core.Data;
using AF.Data.DbContext;
using AF.Domain.Domain.TiMus;

namespace AF.Services.TiMus
{
    /// <summary>
    /// 科目Service
    /// </summary>
    public class SubjectService : ISubjectService
    {
        #region 注入对象
        private readonly IDbContext _dbContext;
        private readonly IRepository<Subject> _subjectRepository;

        public SubjectService(IDbContext dbContext, IRepository<Subject> subjectRepository)
        {
            _dbContext = dbContext;
            _subjectRepository = subjectRepository;
        }

        #endregion

        /// <summary>
        /// 获取科目列表
        /// </summary>
        /// <returns></returns>
        public IList<Subject> GetSubjectList(int? degreeId = null)
        {
            var query = _subjectRepository.Table.Where(d => d.Active);
            if (degreeId.HasValue)
                query = query.Where(d => d.DegreeId == degreeId.Value);
            else
                query = query.Where(d => d.ParentId == null && d.DegreeId == null);
            return query.ToList();
        }
    }
}
