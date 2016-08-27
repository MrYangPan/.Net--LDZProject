using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.TiMus;

namespace AF.Services.TiMus
{
    /// <summary>
    /// 科目interface
    /// </summary>
    public interface ISubjectService
    {
        /// <summary>
        /// 获取科目列表
        /// </summary>
        /// <param name="degreeId">The degree identifier.</param>
        /// <returns></returns>
        IList<Subject> GetSubjectList(int? degreeId = null);
    }
}
