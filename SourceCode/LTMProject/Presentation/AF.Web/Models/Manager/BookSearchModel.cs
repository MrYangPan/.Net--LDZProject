using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Domain.Domain.Books;
using AF.Web.Framework.UI.Paging;
using AF.Web.Models.Book;
using AF.Web.Models.Common;

namespace AF.Web.Models.Manager
{
    public class BookSearchModel: BookCommonModel
    {
        public BookSearchModel()
        {
            PagingFilteringModel=new PagingFilteringModel();
        }

        public IList<Domain.Domain.Books.Book> BookList { get; set; }

        public PagingFilteringModel PagingFilteringModel { get; set; }

    }

}