using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Domain.Domain.BookWork;
using AF.Web.Validators.TaskAllocation;
using FluentValidation.Attributes;

namespace AF.Web.Models.TaskAllocation
{
    [Validator(typeof(EditTaskModelValidator))]
    public class EditTaskModel
    {
        public EditTaskModel()
        {
            StatusItemList = new List<SelectListItem>();
        }

        public int TaskId { get; set; }
        public int? EntryCustomerId { get; set; }
        public int? CheckCustomerId { get; set; }
        public TaskStatus TaskStatus { get; set; }

        public string EntryCustomerName { get; set; }
        public string CheckCustomerName { get; set; }
        public IList<SelectListItem> StatusItemList { get; set; }

    }
}