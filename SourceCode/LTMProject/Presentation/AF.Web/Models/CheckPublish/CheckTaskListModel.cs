using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Domain.Domain.BookWork;

namespace AF.Web.Models.CheckPublish
{
    public class CheckTaskListModel
    {
        public CheckTaskListModel()
        {
            PagingFilteringContext = new CheckTaskListPagingFilteringModel();
            SubjectList = new List<SelectListItem>();
        }

        public IList<BookWorkTask> BookWorkTaskList { get; set; }

        public CheckTaskListPagingFilteringModel PagingFilteringContext { get; set; }

        public IList<SelectListItem> SubjectList { get; set; }

        public int? SubjectId { get; set; }
    }
}