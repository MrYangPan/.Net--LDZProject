using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Web.Framework;
using AF.Web.Models.TaskAllocation;
using FluentValidation;

namespace AF.Web.Validators.TaskAllocation
{
    public class ChapterParamModelValidator : BaseAfValidator<ChapterParamModel>
    {
        public ChapterParamModelValidator()
        {
            RuleFor(x => x.BookId).NotEqual(0).WithMessage("页面错误");
            RuleFor(x => x.BookChapterIds).NotEmpty().WithMessage("请选择章节");
            RuleFor(x => x.EntryCustomerId).NotEmpty().WithMessage("请输入录入人员");
            RuleFor(x => x.CheckCustomerId).NotEmpty().WithMessage("请输入审核人员");
        }
    }
}