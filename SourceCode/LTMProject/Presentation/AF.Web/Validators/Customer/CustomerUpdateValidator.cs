using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Web.Framework;
using AF.Web.Models.Customer;
using FluentValidation;

namespace AF.Web.Validators.Customer
{

    public class CustomerUpdateValidator : BaseAfValidator<CustomerUpdateModel>
    {
        public CustomerUpdateValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.PhoneNumber).Length(11, 11).WithMessage("请输入正确的手机号码");
        }
    }
}