using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Domain.Domain.BookWork;

namespace AF.Web.Models.MarkProperty
{
    public class MarkTaskListModel
    {
        public MarkTaskListModel()
        {
            PagingFilteringContext=new MarkTaskListPagingFilteringModel();
            SubjectList=new List<SelectListItem>();
        }

        public IList<BookWorkTask> BookWorkTaskList { get; set; }

        public MarkTaskListPagingFilteringModel PagingFilteringContext { get; set; }

        public IList<SelectListItem> SubjectList { get; set; }

        public int? SubjectId { get; set; }
    }
}