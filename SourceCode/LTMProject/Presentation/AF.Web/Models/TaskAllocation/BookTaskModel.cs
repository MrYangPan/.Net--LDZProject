using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Web.Models.Book;
using AF.Web.Models.Common;

namespace AF.Web.Models.TaskAllocation
{
    public class BookTaskModel : BookCommonModel
    {
        public BookTaskModel()
        {
            PagingFilteringModel = new PagingFilteringModel();
        }

        public IList<Domain.Domain.BookWork.BookWorkTask> BookWorkTaskList { get; set; }

        public PagingFilteringModel PagingFilteringModel { get; set; }
    }
}