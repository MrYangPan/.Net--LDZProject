using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;
using AF.Core.Data;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;
using AF.Services.Books;
using TaskStatus = AF.Domain.Domain.BookWork.TaskStatus;

namespace AF.Services.BookWork
{
    /// <summary>
    /// 任务Service
    /// </summary>
    public class BookWorkTaskService : IBookWorkTaskService
    {
        private readonly IRepository<BookWorkTask> _bookWorkTaskRepository;
        private readonly IRepository<BookTiMu> _bookTiMuRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookWorkTaskService"/> class.
        /// </summary>
        /// <param name="bookWorkTaskRepository">The book work task repository.</param>
        /// <param name="bookTiMuRepository">The book ti mu repository.</param>
        public BookWorkTaskService(IRepository<BookWorkTask> bookWorkTaskRepository, IRepository<BookTiMu> bookTiMuRepository)
        {
            this._bookWorkTaskRepository = bookWorkTaskRepository;
            _bookTiMuRepository = bookTiMuRepository;
        }

        /// <summary>
        ///  获取一条任务记录
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public BookWorkTask GetBookWorkTask(int id)
        {
            return _bookWorkTaskRepository.GetById(id);
        }

        /// <summary>
        /// 根据教辅编号获取任务列表
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <returns></returns>
        public IList<BookWorkTask> GetBookWorkListByBookId(int bookId)
        {
            return _bookWorkTaskRepository.Table.Where(t => t.BookId == bookId).ToList();
        }

        /// <summary>
        /// 任务列表分页
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public PagedList<BookWorkTask> GetBookWorkList(BookWorkRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var query = _bookWorkTaskRepository.Table;

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(t => t.Book.Name.Contains(request.Name));

            if (!string.IsNullOrEmpty(request.Isbn))
                query = query.Where(t => t.Book.Isbn.Contains(request.Isbn));

            if (request.PublisherId > 0)
                query = query.Where(t => t.Book.PublisherId == request.PublisherId);

            if (request.DegreeId > 0)
                query = query.Where(t => t.Book.DegreeId == request.DegreeId);

            if (request.GradeId > 0)
                query = query.Where(t => t.Book.GradeId == request.GradeId);

            if (request.SubjectId > 0)
                query = query.Where(t => t.Book.SubjectId == request.SubjectId);

            if (request.TermId > 0)
                query = query.Where(t => t.Book.TermId == request.TermId);

            if (request.Year > 0)
                query = query.Where(t => t.Book.Year == request.Year);

            if (request.Begin.HasValue)
            {
                request.Begin = request.Begin.Value.Date;
                query = query.Where(t => t.CreateTime >= request.Begin);
            }

            if (request.End.HasValue)
            {
                request.End = request.End.Value.Date.AddDays(1).AddSeconds(-1);
                query = query.Where(t => t.CreateTime <= request.End);
            }

            if (request.BookId.HasValue)
            {
                query = query.Where(t => t.BookId == request.BookId.Value);
            }

            query = query.OrderByDescending(d => d.CreateTime);
            return new PagedList<BookWorkTask>(query, request.PageIndex, request.PageSize);
        }

        /// <summary>
        /// 任务列表分页
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="taskStatus">Type of the task.</param>
        /// <param name="subjectId">The subject id.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public IPagedList<BookWorkTask> GetAllTasks(int? userId = null, TaskStatus? taskStatus = null, int? subjectId = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _bookWorkTaskRepository.Table;
            if (userId.HasValue)
            {
                query = query.Where(p => p.EntryCustomerId == userId || p.MarkCustomerId == userId || p.CheckCustomerId == userId);
            }
            if (taskStatus.HasValue)
            {
                query = query.Where(d => d.Status == taskStatus);
            }
            if (subjectId.HasValue)
            {
                query = query.Where(p => p.Book.SubjectId == subjectId);
            }
            var bookTaskList = new PagedList<BookWorkTask>(query.OrderByDescending(d => d.CreateTime), pageIndex, pageSize);
            return bookTaskList;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void InsertBookWorkTask(BookWorkTask bookWorkTask)
        {
            if (bookWorkTask == null)
                throw new ArgumentNullException(nameof(bookWorkTask));
            _bookWorkTaskRepository.Insert(bookWorkTask);
        }

        /// <summary>
        /// 删除教辅任务
        /// </summary>
        /// <param name="bookWorkTask">The book work task.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void DeleteBookWorkTask(BookWorkTask bookWorkTask)
        {
            if (bookWorkTask == null)
                throw new ArgumentNullException(nameof(bookWorkTask));
            _bookWorkTaskRepository.Delete(bookWorkTask);
        }

        /// <summary>
        /// 修改教辅任务.
        /// </summary>
        /// <param name="bookWorkTask">The book work task.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void UpdateBookWorkTask(BookWorkTask bookWorkTask)
        {
            if (bookWorkTask == null)
                throw new ArgumentNullException(nameof(bookWorkTask));
            _bookWorkTaskRepository.Update(bookWorkTask);
        }

        /// <summary>
        /// 获取任务包含的所有题目.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        public virtual IList<BookTiMu> GetTaskBookTiMus(int taskId)
        {
            var taskItems = GetBookWorkTask(taskId).BookWorkTaskItems.Select(p => p.Id).ToList();//任务包含的Item
            var bookTiMus = _bookTiMuRepository.Table.Where(p => taskItems.Contains(p.TaskItemId)).ToList();
            return bookTiMus;
        }

        /// <summary>
        /// 提交任务.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void SubmitTask(BookWorkTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            _bookWorkTaskRepository.Update(task);
        }

        /// <summary>
        /// 审核标记题目错误与否.
        /// </summary>
        /// <param name="bookTiMu">The tmid.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void SignTmWrong(BookTiMu bookTiMu)
        {
            if (bookTiMu == null)
            {
                throw new ArgumentNullException(nameof(bookTiMu));
            }
            _bookTiMuRepository.Update(bookTiMu);
        }
    }
}
