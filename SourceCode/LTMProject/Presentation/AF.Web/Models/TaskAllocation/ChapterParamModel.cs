using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Domain.Domain.Books;
using AF.Domain.Domain.BookWork;
using AF.Web.Validators.TaskAllocation;
using FluentValidation.Attributes;

namespace AF.Web.Models.TaskAllocation
{
    [Validator(typeof(ChapterParamModelValidator))]
    public class ChapterParamModel
    {
        public ChapterParamModel()
        {
            BookChapterList = new List<BookChapter>();
        }

        public int BookId { get; set; }
        public int? EntryCustomerId { get; set; }
        public int? MarkCustomerId { get; set; }
        public int? CheckCustomerId { get; set; }

        public string BookChapterIds { get; set; }

        public IList<BookChapter> BookChapterList { get; set; }

         
    }
}