using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core.Data;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.TiMus;
using AF.Services.Books.Compose;

namespace AF.Services.Books
{
    /// <summary>
    /// 章节Service
    /// </summary>
    public class BookChapterService : IBookChapterService
    {
        #region 对象注入
        private readonly IRepository<BookChapter> _bookChapterRepository;
        private readonly IRepository<BookTiMu> _bookTiMuRepository;
        private readonly IRepository<TiMu> _tiMuRepository;

        public BookChapterService(IRepository<BookChapter> bookChapterRepository, IRepository<BookTiMu> bookTiMuRepository, IRepository<TiMu> tiMuRepository)
        {
            _bookChapterRepository = bookChapterRepository;
            _bookTiMuRepository = bookTiMuRepository;
            _tiMuRepository = tiMuRepository;
        }

        #endregion


        /// <summary>
        /// Updates the book chapter.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public void UpdateBookChapter(BookChapter entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _bookChapterRepository.Update(entity);
        }

        /// <summary>
        /// Inserts the book chapter.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void InsertBookChapter(BookChapter entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _bookChapterRepository.Insert(entity);
        }

        /// <summary>
        /// Deletes the book chapter.
        /// </summary>
        /// <param name="entitys">The entity.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void DeleteBookChapter(params BookChapter[] entitys)
        {
            if (entitys == null)
            {
                throw new ArgumentNullException(nameof(entitys));
            }
            _bookChapterRepository.Delete(entitys);
        }



        /// <summary>
        /// Gets the book chapter by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public BookChapter GetBookChapterById(int id)
        {
            return _bookChapterRepository.GetById(id);
        }

        /// <summary>
        /// Gets the book chapters.
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <returns></returns>
        public IList<BookChapter> GetBookChapters(int bookId)
        {
            return _bookChapterRepository.Table.Where(d => d.ParentId == null && bookId == d.BookId).ToList();
        }

        /// <summary>
        /// 根据任务子项获取题目集合（章节-任务子项：是一对一的关系）
        /// </summary>
        /// <param name="taskItemId">The task item identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<TiMu> GeTiMusByintBookTaskItemId(int taskItemId)
        {
            var timuIdList = _bookTiMuRepository.Table.Where(d => d.TaskItemId == taskItemId).Select(d => d.Id);
            var timuList = _tiMuRepository.Table.Where(d => timuIdList.Contains(d.Id)).Distinct();
            return timuList.ToList();
        }

        /// <summary>
        /// 根据章节id，递归查找当前章节的所有父章节，保存到集合
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public IList<BookChapter> GetParentListById(int id)
        {
            var bookChapterList = new List<BookChapter>();
            var chapter = _bookChapterRepository.GetById(id);
            bookChapterList.Add(chapter);
            //递归
            RecursionParent(chapter, bookChapterList);
            //反转顺序
            bookChapterList.Reverse();
            return bookChapterList;
        }

        /// <summary>
        /// 根据父级编号获取子集
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        public IList<BookChapter> GetBookChapterByParentId(int parentId)
        {
            return _bookChapterRepository.Table.Where(t => t.ParentId == parentId).ToList();
        }

        #region 公用
        /// <summary>
        /// 递归查找当前章节的所有父章节，保存到集合
        /// </summary>
        /// <param name="bookChapter">The book chapter.</param>
        /// <param name="bookChapterList">The book chapter list.</param>
        private void RecursionParent(BookChapter bookChapter, IList<BookChapter> bookChapterList)
        {
            if (bookChapter.BookChapterParent != null)
            {
                bookChapterList.Add(bookChapter.BookChapterParent);
                RecursionParent(bookChapter.BookChapterParent, bookChapterList);
            }
        }
        #endregion
    }
}
