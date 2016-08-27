using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Web.Framework;
using AF.Web.Models.Manager;
using FluentValidation;

namespace AF.Web.Validators.Manager
{
    public class SetBookValidator : BaseAfValidator<SetBookModel>
    {
        public SetBookValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("书名不能为空");
            RuleFor(x => x.DegreeId).NotEqual(0).WithMessage("请选择学段");
            RuleFor(x => x.SubjectId).NotEqual(0).WithMessage("请选择学科");

            //RuleFor(x => x.Isbn).NotEmpty().WithMessage("ISBN不能为空");
            //RuleFor(x => x.PublisherId).NotEqual(0).WithMessage("请选择出版社");
            //RuleFor(x => x.TermId).NotEqual(0).WithMessage("请选择学期");

            //RuleFor(x => x.GradeId).NotEqual(0).WithMessage("请选择年级");
            //RuleFor(x => x.Year).NotEqual(0).WithMessage("请选择年份");
        }
    }
}