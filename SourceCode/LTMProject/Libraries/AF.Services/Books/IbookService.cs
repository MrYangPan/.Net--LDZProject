using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;
using AF.Domain.Domain.Books;

namespace AF.Services.Books
{
    public interface IBookService
    {
        /// <summary>
        /// 获取教辅
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        IPagedList<Book> GetBookPage(BookRequest request);



        /// <summary>
        /// Gets the book by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Book GetBookById(int id);

        /// <summary>
        /// Inserts the book.
        /// </summary>
        /// <param name="model">The model.</param>
        void InsertBook(Book model);


        /// <summary>
        /// Updates the book.
        /// </summary>
        /// <param name="model">The model.</param>
        void UpdateBook(Book model);

        /// <summary>
        /// Deletes the book.
        /// </summary>
        /// <param name="book">The book.</param>
        void DeleteBook(Book book);


        /// <summary>
        /// Gets the degree list.
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Gets the degree list.
        /// </summary>
        /// <returns></returns>
        IList<Grade> GetGradeList(int? degreeId = null);


        /// <summary>
        /// Gets the degree list.
        /// </summary>
        /// <returns></returns>
        IList<Degree> GetDegreeList();


        /// <summary>
        /// Gets all exa years.
        /// </summary>
        /// <returns></returns>
        IList<ExaYear> GetAllExaYears();

        /// <summary>
        /// Gets all publishers.
        /// </summary>
        /// <returns></returns>
        IList<Publisher> GetAllPublishers();



        /// <summary>
        /// Gets all terms.
        /// </summary>
        /// <returns></returns>
        IList<Term> GetAllTerms();
    }
}
