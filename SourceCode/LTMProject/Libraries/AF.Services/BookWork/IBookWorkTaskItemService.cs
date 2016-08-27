using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;

namespace AF.Services.BookWork
{
    /// <summary>
    /// 子任务interface
    /// </summary>
    public interface IBookWorkTaskItemService
    {
        /// <summary>
        /// 根据任务id获取所有任务子项
        /// </summary>
        /// <param name="taskid">The book work taskid.</param>
        /// <returns></returns>
        IList<BookWorkTaskItem> GetBookWorkTaskItemsByTaskId(int taskid);

        /// <summary>
        /// 获取任务题目.
        /// </summary>
        /// <param name="taskitemId">The taskitem identifier.</param>
        /// <returns></returns>
        IList<BookTiMu> GetBookTiMus(int taskitemId);

        /// <summary>
        /// 获取BookTimu.
        /// </summary>
        /// <param name="tmid">The tmid.</param>
        /// <returns></returns>
        BookTiMu GetBookTiMuById(Guid tmid);

        /// <summary>
        /// Gets the book work task items by book identifier.
        /// </summary>
        /// <param name="bookid">The bookid.</param>
        /// <returns></returns>
        IList<BookWorkTaskItem> GetBookWorkTaskItemsByBookId(int bookid);

        /// <summary>
        /// Gets the book work task items by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        BookWorkTaskItem GetBookWorkTaskItemsById(int id);

        /// <summary>
        /// 添加任务节点
        /// </summary>
        /// <param name="items">The items.</param>
        void InsertBookWorkTaskItems(IList<BookWorkTaskItem> items);

        /// <summary>
        /// 删除任务节点
        /// </summary>
        /// <param name="items">The items.</param>
        void DeleteBookWorkTaskItems(IList<BookWorkTaskItem> items);

        /// <summary>
        /// Updates the book work task item.
        /// </summary>
        /// <param name="bookWorkTaskItem">The book work task item.</param>
        void UpdateBookWorkTaskItem(BookWorkTaskItem bookWorkTaskItem);

    }

}
