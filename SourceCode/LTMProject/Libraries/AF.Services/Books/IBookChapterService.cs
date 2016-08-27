using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.TiMus;
using AF.Services.Books.Compose;

namespace AF.Services.Books
{
    /// <summary>
    /// 章节interface
    /// </summary>
    public interface IBookChapterService
    {

        /// <summary>
        /// Gets the book chapter by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        BookChapter GetBookChapterById(int id);


        /// <summary>
        /// Deletes the book chapter.
        /// </summary>
        /// <param name="entitys">The entitys.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        void DeleteBookChapter(params BookChapter[] entitys);


        /// <summary>
        /// Updates the book chapter.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void UpdateBookChapter(BookChapter entity);


        /// <summary>
        /// Inserts the book chapter.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        void InsertBookChapter(BookChapter entity);

        /// <summary>
        /// Gets the book chapters.
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <returns></returns>
        IList<BookChapter> GetBookChapters(int bookId);

        /// <summary>
        /// 根据任务子项获取题目集合（章节-任务子项：是一对一的关系）
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <returns></returns>
        IList<TiMu> GeTiMusByintBookTaskItemId(int bookId);

        /// <summary>
        /// 根据章节id，递归查找当前章节的所有父章节，保存到集合
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        IList<BookChapter> GetParentListById(int id);

        /// <summary>
        /// 根据父级编号获取子集
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        IList<BookChapter> GetBookChapterByParentId(int parentId);
    }
}
