using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Web.Framework;
using AF.Web.Models.TaskAllocation;
using FluentValidation;

namespace AF.Web.Validators.TaskAllocation
{
    public class EditTaskModelValidator : BaseAfValidator<EditTaskModel>
    {
        public EditTaskModelValidator()
        {
            RuleFor(x => x.TaskId).NotEqual(0).WithMessage("任务编号不能为空");
            RuleFor(x => x.EntryCustomerId).NotEmpty().WithMessage("请输入录入人员");
            RuleFor(x => x.CheckCustomerId).NotEmpty().WithMessage("请输入审核人员");
        }
    }
}