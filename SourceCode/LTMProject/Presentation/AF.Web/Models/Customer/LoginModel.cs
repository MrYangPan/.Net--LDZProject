using System.ComponentModel.DataAnnotations;
using AF.Web.Validators.Customer;
using FluentValidation.Attributes;

namespace AF.Web.Models.Customer
{
    [Validator(typeof(LoginValidator))]
    public class LoginModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}