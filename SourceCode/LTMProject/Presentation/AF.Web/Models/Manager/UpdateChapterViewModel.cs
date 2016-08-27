using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Web.Framework;
using AF.Web.Framework.UI.Paging;
using AF.Web.Validators.Manager;
using FluentValidation.Attributes;

namespace AF.Web.Models.Manager
{
    [Validator(typeof(UpdateValidator))]
    public class UpdateChapterViewModel: BaseEntityModel
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public int BookId { get; set; }
    }
}