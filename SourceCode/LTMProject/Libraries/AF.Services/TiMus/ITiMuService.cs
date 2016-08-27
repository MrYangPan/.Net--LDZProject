using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.TiMus;
using AF.Services.BookWork;

namespace AF.Services.TiMus
{
    /// <summary>
    /// 题目interface
    /// </summary>
    public interface ITiMuService
    {
        /// <summary>
        /// 根据题目id获取题目对象
        /// </summary>
        /// <param name="tmid">The tmid.</param>
        /// <returns></returns>
        TiMu GeTiMuById(Guid tmid);

        /// <summary>
        /// 添加题目
        /// </summary>
        /// <param name="tiMu">The ti mu.</param>
        void InsertTiMu(TiMu tiMu);

        /// <summary>
        /// 题目分页
        /// </summary>
        /// <param name="chapterId">The chapter identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        IPagedList<TiMu> GetTiMuPagedList(int chapterId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="tiMuImages">The ti mu images.</param>
        void InsertImage(TiMuImages tiMuImages);

        /// <summary>
        /// 添加答案
        /// </summary>
        /// <param name="input">The input.</param>
        void InsertInput(Input input);

        /// <summary>
        /// 添加答案选项
        /// </summary>
        /// <param name="inputChoice">The input choice.</param>
        void InsertInputChoice(InputChoice inputChoice);

        /// <summary>
        /// 根据guid获取竖式
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        ShushiTrack GetShushiById(Guid guid);

        /// <summary>
        /// 解析题目（选项、分析、解答、点评等）并且保存到相应的表
        /// </summary>
        /// <param name="timuStr">The timu string.</param>
        /// <param name="taskItemId">The task item identifier.</param>
        /// <param name="parentTmId">The parent tm identifier.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        string SaveTiMu(string timuStr, int? taskItemId, Guid? parentTmId, string property = null);

        /// <summary>
        /// 根据科目id获取所有题型
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        IList<TiMuType> GetAllTypes(int subjectId);

        /// <summary>
        /// 根据选项id，删除选项
        /// </summary>
        /// <param name="choiceId">The choice identifier.</param>
        void DeleteChoice(Guid? choiceId);

        /// <summary>
        /// 录题页面编辑使用
        /// </summary>
        /// <param name="tmid">The tmid.</param>
        /// <returns></returns>
        TiMu EntryGeTiMuFormatById(Guid tmid);

        /// <summary>
        /// 保存题目标定的属性
        /// </summary>
        /// <param name="property">The property.</param>
        string SaveTiMuMark(string property = null);

        /// <summary>
        /// 删除题目
        /// </summary>
        /// <param name="models">The models.</param>
        void DeleteTiMu(params TiMu[] models);

        /// <summary>
        /// Updates the ti mu.
        /// </summary>
        /// <param name="tiMu">The ti mu.</param>
        void UpdateTiMu(TiMu tiMu);

        #region BookTiMu
        /// <summary>
        /// 根据书本id获取BookTiMu
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <returns></returns>
        IList<BookTiMu> GetBookTiMuList(int bookId);

        /// <summary>
        /// 删除BookTiMu
        /// </summary>
        /// <param name="model">The model.</param>
        void DeleteBookTiMu(params BookTiMu[] models);
        #endregion
    }
}
