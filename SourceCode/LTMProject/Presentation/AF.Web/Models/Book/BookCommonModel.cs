using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Web.Framework.UI.Paging;

namespace AF.Web.Models.Book
{
    public class BookCommonModel
    {
        public BookCommonModel()
        {
            SubjectItemList = new List<SelectListItem>();
            DegreeItemList = new List<SelectListItem>();
            GradeItemList = new List<SelectListItem>();
            TermItemList = new List<SelectListItem>();
            YearItemList = new List<SelectListItem>();
            PublishItemList = new List<SelectListItem>();
        }

        public string Isbn { get; set; }
        public string Name { get; set; }
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }

        public int SubjectId { get; set; }
        public int PublisherId { get; set; }
        public int DegreeId { get; set; }
        public int GradeId { get; set; }
        public int TermId { get; set; }
        public int Year { get; set; }

        public IList<SelectListItem> SubjectItemList { get; set; }
        public IList<SelectListItem> PublishItemList { get; set; }
        public IList<SelectListItem> DegreeItemList { get; set; }
        public IList<SelectListItem> GradeItemList { get; set; }
        public IList<SelectListItem> TermItemList { get; set; }
        public IList<SelectListItem> YearItemList { get; set; }
    }
}