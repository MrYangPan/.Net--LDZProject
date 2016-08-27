using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using AF.Core;
using AF.Core.Caching;
using AF.Core.Data;
using AF.Domain.Domain.Books;
using AF.Services.BookWork;
using AF.Services.TiMus;

namespace AF.Services.Books
{
    public class BookService : IBookService
    {
        #region Fields

        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Degree> _degreeRepository;
        private readonly IRepository<Grade> _gradeRepository;
        private readonly IRepository<ExaYear> _exayearRepository;
        private readonly IRepository<Publisher> _publisherRepository;
        private readonly IRepository<Term> _termRepository;

        private readonly IBookWorkTaskService _bookWorkTaskService;
        private readonly IBookWorkTaskItemService _bookWorkTaskItemService;
        private readonly ITiMuService _tiMuService;

        #endregion  

        #region Ctor

        public BookService(IRepository<Book> bookRepository, IRepository<Degree> degreeRepository,
            IRepository<Grade> gradeRepository, IRepository<ExaYear> exayearRepository,
            IRepository<Publisher> publisherRepository, IRepository<Term> termRepository,
            IBookWorkTaskService bookWorkTaskService, IBookWorkTaskItemService bookWorkTaskItemService,
            ITiMuService tiMuService
            )
        {
            _bookRepository = bookRepository;
            _degreeRepository = degreeRepository;
            _gradeRepository = gradeRepository;
            _exayearRepository = exayearRepository;
            _publisherRepository = publisherRepository;
            _termRepository = termRepository;
            _bookWorkTaskService = bookWorkTaskService;
            _bookWorkTaskItemService = bookWorkTaskItemService;
            _tiMuService = tiMuService;
        }

        #endregion

        /// <summary>
        /// 获取教辅
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public IPagedList<Book> GetBookPage(BookRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var qurey = _bookRepository.Table;

            if (!string.IsNullOrEmpty(request.Name))
                qurey = qurey.Where(t => t.Name.Contains(request.Name));

            if (!string.IsNullOrEmpty(request.Isbn))
                qurey = qurey.Where(t => t.Isbn.Contains(request.Isbn));

            if (request.PublisherId > 0)
                qurey = qurey.Where(t => t.PublisherId == request.PublisherId);

            if (request.DegreeId > 0)
                qurey = qurey.Where(t => t.DegreeId == request.DegreeId);

            if (request.GradeId > 0)
                qurey = qurey.Where(t => t.GradeId == request.GradeId);

            if (request.SubjectId > 0)
                qurey = qurey.Where(t => t.SubjectId == request.SubjectId);

            if (request.TermId > 0)
                qurey = qurey.Where(t => t.TermId == request.TermId);

            if (request.Year > 0)
                qurey = qurey.Where(t => t.Year == request.Year);

            if (request.Begin.HasValue)
                qurey = qurey.Where(t => t.CreateTime >= request.Begin);

            if (request.End.HasValue)
                qurey = qurey.Where(t => t.CreateTime <= request.End);

            qurey = qurey.OrderByDescending(d => d.CreateTime);
            return new PagedList<Book>(qurey, request.PageIndex, request.PageSize);
        }

        /// <summary>
        /// Gets the book by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Book GetBookById(int id)
        {
            return _bookRepository.GetById(id);
        }

        /// <summary>
        /// Inserts the book.
        /// </summary>
        /// <param name="book">The book.</param>
        public void InsertBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException("book");
            }
            _bookRepository.Insert(book);
        }

        /// <summary>
        /// Updates the book.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <exception cref="System.ArgumentNullException">book</exception>
        public void UpdateBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException("book");
            }

            _bookRepository.Update(book);
        }

        /// <summary>
        /// Deletes the book.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <exception cref="ArgumentNullException">book</exception>
        public void DeleteBook(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            _bookRepository.Delete(book);

            //删除书本的同时删除任务
            var taskList = _bookWorkTaskService.GetBookWorkList(new BookWorkRequest() { BookId = book.Id });
            taskList?.ForEach(t => _bookWorkTaskService.DeleteBookWorkTask(t));

            //删除书本目录节点
            var itemList = _bookWorkTaskItemService.GetBookWorkTaskItemsByBookId(book.Id);
            _bookWorkTaskItemService.DeleteBookWorkTaskItems(itemList);

            //删除书本关联的 题目
            var bookTiMuList = _tiMuService.GetBookTiMuList(book.Id);
            _tiMuService.DeleteTiMu(bookTiMuList?.Select(t => t.Timu).ToArray());
            _tiMuService.DeleteBookTiMu(bookTiMuList?.ToArray());
        }



        /// <summary>
        /// Gets the degree list.
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Gets the degree list.
        /// </summary>
        /// <returns></returns>
        public virtual IList<Grade> GetGradeList(int? degreeId = null)
        {
            var query = _gradeRepository.Table;
            if (degreeId.HasValue)
            {
                query = query.Where(d => d.DegreeId == degreeId);
            }
            return query.ToList();
        }


        /// <summary>
        /// Gets the degree list.
        /// </summary>
        /// <returns></returns>
        public virtual IList<Degree> GetDegreeList()
        {
            var query = _degreeRepository.Table.Where(t => t.Active);
            return query.ToList();
        }


        /// <summary>
        /// Gets all exa years.
        /// </summary>
        /// <returns></returns>
        public virtual IList<ExaYear> GetAllExaYears()
        {
            var query = _exayearRepository.Table;
            return query.ToList();
        }


        /// <summary>
        /// Gets all publishers.
        /// </summary>
        /// <returns></returns>
        public virtual IList<Publisher> GetAllPublishers()
        {
            var query = _publisherRepository.Table;
            return query.ToList();
        }


        /// <summary>
        /// Gets all terms.
        /// </summary>
        /// <returns></returns>
        public virtual IList<Term> GetAllTerms()
        {
            var query = _termRepository.Table;
            return query.ToList();
        }
    }

}
