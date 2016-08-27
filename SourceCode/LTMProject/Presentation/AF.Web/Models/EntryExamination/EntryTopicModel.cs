using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Domain.Domain.Books;

namespace AF.Web.Models.EntryExamination
{
    public class EntryTopicModel
    {
        public int taskItemId { get; set; }

        public int taskId { get; set; }
        public bool? revert { get; set; }
        public string timuId { get; set; }
        public bool? isEdit { get; set; }
        public int? order { get; set; }
        public int? cycles { get; set; }
        public BookTiMu.TimuStatus? timuStatus { get; set; }
        public int? childCycle { get; set; }
        public string revertType { get; set; }
    }
}