using AF.Web.Framework;
using AF.Web.Models.Customer;
using FluentValidation;

namespace AF.Web.Validators.Customer
{
    public class LoginValidator : BaseAfValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
        }
    }
}