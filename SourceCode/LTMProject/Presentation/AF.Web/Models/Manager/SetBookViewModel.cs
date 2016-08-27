using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Web.Models.Book;
using AF.Web.Validators.Manager;
using FluentValidation.Attributes;

namespace AF.Web.Models.Manager
{
    public class SetBookViewModel: BookCommonModel
    {
        public SetBookViewModel()
        {

        }

        public string Version { get; set; }
    }

    [Validator(typeof(SetBookValidator))]
    public class SetBookModel : BookCommonModel
    {
        public int Id { get; set; }
        public string Version { get; set; }
    }


}