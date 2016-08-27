using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core.Data;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;

namespace AF.Services.BookWork
{
    /// <summary>
    /// 子任务Service
    /// </summary>
    public class BookWorkTaskItemService : IBookWorkTaskItemService
    {
        #region 注入对象
        private readonly IRepository<BookWorkTask> _bookWorkTaskRepository;
        private readonly IRepository<BookWorkTaskItem> _bookWorkTaskItemRepository;
        private readonly IRepository<BookChapter> _bookChapterRepository;
        private readonly IRepository<BookTiMu> _bookTiMuRepository;
        public BookWorkTaskItemService(
            IRepository<BookWorkTaskItem> bookWorkTaskItemRepository,
            IRepository<BookChapter> bookChapterRepository,
            IRepository<BookWorkTask> bookWorkTaskRepository, 
            IRepository<BookTiMu> bookTiMuRepository
            )
        {
            _bookWorkTaskItemRepository = bookWorkTaskItemRepository;
            _bookChapterRepository = bookChapterRepository;
            _bookWorkTaskRepository = bookWorkTaskRepository;
            _bookTiMuRepository = bookTiMuRepository;
        }

        #endregion

        /// <summary>
        /// 根据任务id获取所有任务子项
        /// </summary>
        /// <param name="taskid">The book work taskid.</param>
        /// <returns></returns>
        public IList<BookWorkTaskItem> GetBookWorkTaskItemsByTaskId(int taskid)
        {
            return _bookWorkTaskItemRepository.Table.Where(d => d.TaskId == taskid).ToList();
        }

        /// <summary>
        /// 获取BookTimu.
        /// </summary>
        /// <param name="tmid">The tmid.</param>
        /// <returns></returns>
        public BookTiMu GetBookTiMuById(Guid tmid)
        {
            return _bookTiMuRepository.Table.FirstOrDefault(p => p.Id == tmid);
        }

        /// <summary>
        /// Gets the book work task items by book identifier.
        /// </summary>
        /// <param name="bookid">The bookid.</param>
        /// <returns></returns>
        public virtual IList<BookWorkTaskItem> GetBookWorkTaskItemsByBookId(int bookid)
        {
            var query = from bti in _bookWorkTaskItemRepository.Table
                        join bt in _bookWorkTaskRepository.Table on bti.TaskId equals bt.Id
                        where bt.BookId == bookid
                        select bti;
            return query.ToList();
        }

        /// <summary>
        /// Gets the book work task items by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public BookWorkTaskItem GetBookWorkTaskItemsById(int id)
        {
            return _bookWorkTaskItemRepository.GetById(id);
        }

        /// <summary>
        /// 添加任务节点
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public void InsertBookWorkTaskItems(IList<BookWorkTaskItem> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
            {
                _bookWorkTaskItemRepository.Insert(item);
            }
        }

        /// <summary>
        /// 删除任务节点
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="System.ArgumentNullException">items</exception>
        public void DeleteBookWorkTaskItems(IList<BookWorkTaskItem> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
            {
                _bookWorkTaskItemRepository.Delete(item);
            }
        }

        /// <summary>
        /// Updates the book work task item.
        /// </summary>
        /// <param name="bookWorkTaskItem">The book work task item.</param>
        public void UpdateBookWorkTaskItem(BookWorkTaskItem bookWorkTaskItem)
        {
            _bookWorkTaskItemRepository.Update(bookWorkTaskItem);
        }

        /// <summary>
        /// 获取任务题目.
        /// </summary>
        /// <param name="taskitemId">The taskitem identifier.</param>
        /// <returns></returns>
        public virtual IList<BookTiMu> GetBookTiMus(int taskitemId)
        {
            var bookTiMus = _bookTiMuRepository.Table.Where(p => p.TaskItemId == taskitemId && p.Timu.ParentId == null).OrderBy(d=>d.Order).ToList();
            return bookTiMus;
        }
    }
}
