using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AF.Web.Models.Knowledge
{
    public class KnowledgeModel
    {
        public KnowledgeModel()
        {
            DegreeItemList = new List<SelectListItem>();
            SubjectItemList = new List<SelectListItem>();
        }

        public int DegreeId { get; set; }
        public IList<SelectListItem> DegreeItemList { get; set; }
        public int SubjectId { get; set; }
        public IList<SelectListItem> SubjectItemList { get; set; }
    }
}