using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Web.Framework;
using AF.Web.Models.Customer;
using FluentValidation;

namespace AF.Web.Validators.Customer
{
    public class CustomerCreateValidator : BaseAfValidator<CustomerCreateModel>
    {
        public CustomerCreateValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.PassWord).NotEmpty().WithMessage("密码不能为空");
            RuleFor(x => x.PhoneNumber).Length(11).WithMessage("请输入正确的手机号码");
        }
    }
}
