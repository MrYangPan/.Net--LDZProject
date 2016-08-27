using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Web.Framework;
using AF.Web.Models.Manager;
using FluentValidation;

namespace AF.Web.Validators.Manager
{


    public class UpdateValidator : BaseAfValidator<UpdateChapterViewModel>
    {
        public UpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("章节名不能为空");
            //RuleFor(x => x.QuesCount).GreaterThan(0).WithMessage("题目数必须大于0");
        }
    }
}