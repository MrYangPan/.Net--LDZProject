using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;
using TaskStatus = AF.Domain.Domain.BookWork.TaskStatus;

namespace AF.Services.BookWork
{
    /// <summary>
    /// 任务interface
    /// </summary>
    public interface IBookWorkTaskService
    {
        /// <summary>
        /// 获取一条任务记录
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        BookWorkTask GetBookWorkTask(int id);

        /// <summary>
        /// 根据教辅编号获取任务列表
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <returns></returns>
        IList<BookWorkTask> GetBookWorkListByBookId(int bookId);

        /// <summary>
        /// 任务列表分页
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        PagedList<BookWorkTask> GetBookWorkList(BookWorkRequest request);

        /// <summary>
        /// 任务列表分页
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="taskStatus">Type of the task.</param>
        /// <param name="subjectId">The subject id.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        IPagedList<BookWorkTask> GetAllTasks(int? userId = null, TaskStatus? taskStatus = null, int? subjectId = null, int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// 添加任务
        /// </summary>
        void InsertBookWorkTask(BookWorkTask bookWorkTask);

        /// <summary>
        /// 删除教辅任务
        /// </summary>
        /// <param name="bookWorkTask">The book work task.</param>
        void DeleteBookWorkTask(BookWorkTask bookWorkTask);

        /// <summary>
        /// 修改教辅任务.
        /// </summary>
        /// <param name="bookWorkTask">The book work task.</param>
        void UpdateBookWorkTask(BookWorkTask bookWorkTask);

        /// <summary>
        /// 获取任务包含的所有题目.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        IList<BookTiMu> GetTaskBookTiMus(int taskId);

        /// <summary>
        /// 提交任务.
        /// </summary>
        /// <param name="task">The task.</param>
        void SubmitTask(BookWorkTask task);

        /// <summary>
        /// 审核标记题目错误与否.
        /// </summary>
        /// <param name="bookTiMu">The tmid.</param>
        void SignTmWrong(BookTiMu bookTiMu);
    }
}
