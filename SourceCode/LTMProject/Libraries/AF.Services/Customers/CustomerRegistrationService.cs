using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Hosting;
using AF.Core;
using AF.Domain.Domain.Customers;
using AF.Services.Security;

namespace AF.Services.Customers
{
    /// <summary>
    /// Customer registration service
    /// </summary>
    public partial class CustomerRegistrationService : ICustomerRegistrationService
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;

        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customerService">Customer service</param>
        /// <param name="encryptionService">Encryption service</param>
        public CustomerRegistrationService(ICustomerService customerService,
            IEncryptionService encryptionService,  IWebHelper webHelper)
        {
            this._customerService = customerService;
            this._encryptionService = encryptionService;

            _webHelper = webHelper;
        }

        #endregion

        #region Methods



        /// <summary>
        /// Validate customer
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">Password</param>
        /// <returns>
        /// Result
        /// </returns>
        public virtual CustomerLoginResults ValidateCustomer(string username, string password)
        {
            Customer customer = _customerService.GetCustomerByUsername(username);


            if (customer == null)
                return CustomerLoginResults.CustomerNotExist;
            if (customer.Deleted)
                return CustomerLoginResults.Deleted;
            if (!customer.Active)
                return CustomerLoginResults.NotActive;

            string pwd = "";
            switch (customer.PasswordFormat)
            {
                case PasswordFormat.Encrypted:
                    pwd = _encryptionService.EncryptText(password);
                    break;
                case PasswordFormat.Hashed:
                    pwd = _encryptionService.CreatePasswordHash(password, customer.PasswordSalt);
                    break;
                default:
                    pwd = password;
                    break;
            }

            bool isValid = pwd == customer.Password;
            if (!isValid)
                return CustomerLoginResults.WrongPassword;

            //save last login date
            customer.LastLoginDate = DateTime.Now;
            _customerService.UpdateCustomer(customer);

            return CustomerLoginResults.Successful;
        }

        /// <summary>
        /// Register customer
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Customer == null)
                throw new ArgumentException("Can't load current customer");

            var result = new CustomerRegistrationResult();


            if (String.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("密码不能为空");
                return result;
            }

            if (_customerService.GetCustomerByUsername(request.Username) != null)
            {
                result.AddError("用户名已存在");
                return result;
            }

            //at this point request is valid
            request.Customer.Username = request.Username;
            request.Customer.PasswordFormat = request.PasswordFormat;

            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    {
                        request.Customer.Password = request.Password;
                    }
                    break;
                case PasswordFormat.Encrypted:
                    {
                        request.Customer.Password = _encryptionService.EncryptText(request.Password);
                    }
                    break;
                case PasswordFormat.Hashed:
                    {
                        string saltKey = _encryptionService.CreateSaltKey(5);
                        request.Customer.PasswordSalt = saltKey;
                        request.Customer.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey);
                    }
                    break;
                default:
                    break;
            }


            _customerService.InsertCustomer(request.Customer);
            return result;
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual ChangePasswordResult ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var result = new ChangePasswordResult();
            if (String.IsNullOrWhiteSpace(request.UserName))
            {
                result.AddError("用户名不能为空");
                return result;
            }
            if (String.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError("新密码不能为空");
                return result;
            }

            var customer = _customerService.GetCustomerByUsername(request.UserName);
            if (customer == null)
            {
                result.AddError("用户不存在");
                return result;
            }


            var requestIsValid = false;
            if (request.ValidateRequest)
            {
                //password
                string oldPwd = "";
                switch (customer.PasswordFormat)
                {
                    case PasswordFormat.Encrypted:
                        oldPwd = _encryptionService.EncryptText(request.OldPassword);
                        break;
                    case PasswordFormat.Hashed:
                        oldPwd = _encryptionService.CreatePasswordHash(request.OldPassword, customer.PasswordSalt);
                        break;
                    default:
                        oldPwd = request.OldPassword;
                        break;
                }

                bool oldPasswordIsValid = oldPwd == customer.Password;
                if (!oldPasswordIsValid)
                    result.AddError("原密码不正确");

                if (oldPasswordIsValid)
                    requestIsValid = true;
            }
            else
                requestIsValid = true;


            //at this point request is valid
            if (requestIsValid)
            {
                switch (request.NewPasswordFormat)
                {
                    case PasswordFormat.Clear:
                        {
                            customer.Password = request.NewPassword;
                        }
                        break;
                    case PasswordFormat.Encrypted:
                        {
                            customer.Password = _encryptionService.EncryptText(request.NewPassword);
                        }
                        break;
                    case PasswordFormat.Hashed:
                        {
                            string saltKey = _encryptionService.CreateSaltKey(5);
                            customer.PasswordSalt = saltKey;
                            customer.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey);
                        }
                        break;
                    default:
                        break;
                }
                customer.PasswordFormat = request.NewPasswordFormat;
                _customerService.UpdateCustomer(customer);
            }

            return result;
        }

        /// <summary>
        /// Forgots password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="passwordFormat">The password format.</param>
        /// <returns></returns>
        public virtual ForgotPasswordResult ForgotPassword(string username, string newPassword,
            PasswordFormat passwordFormat)
        {
            var result = new ForgotPasswordResult();
            var customer = _customerService.GetCustomerByUsername(username);
            if (customer == null)
            {
                result.AddError("用户不存在");
                return result;
            }

            switch (passwordFormat)
            {
                case PasswordFormat.Clear:
                    {
                        customer.Password = username;
                    }
                    break;
                case PasswordFormat.Encrypted:
                    {
                        customer.Password = _encryptionService.EncryptText(username);
                    }
                    break;
                case PasswordFormat.Hashed:
                    {
                        string saltKey = _encryptionService.CreateSaltKey(5);
                        customer.PasswordSalt = saltKey;
                        customer.Password = _encryptionService.CreatePasswordHash(newPassword, saltKey);
                    }
                    break;
                default:
                    break;
            }
            customer.PasswordFormat = passwordFormat;
            _customerService.UpdateCustomer(customer);
            return result;
        }

        /// <summary>
        /// Sets a user email
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newEmail">New email</param>
        public virtual void SetEmail(Customer customer, string newEmail)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (newEmail == null)
                throw new AFException("Email cannot be null");

            newEmail = newEmail.Trim();

            if (!CommonHelper.IsValidEmail(newEmail))
                throw new AFException("Account.EmailErrors.NewEmailIsNotValid");

            if (newEmail.Length > 100)
                throw new AFException("Account.EmailErrors.EmailTooLong");

            customer.Email = newEmail;
            _customerService.UpdateCustomer(customer);

        }

        /// <summary>
        /// Sets a customer username
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="newUsername">New Username</param>
        public virtual void SetUsername(Customer customer, string newUsername)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            newUsername = newUsername.Trim();

            if (newUsername.Length > 100)
                throw new AFException("Account.UsernameErrors.UsernameTooLong");

            var user2 = _customerService.GetCustomerByUsername(newUsername);
            if (user2 != null && customer.Id != user2.Id)
                throw new AFException("Account.EmailUsernameErrors.UsernameAlreadyExists");

            customer.Username = newUsername;
            _customerService.UpdateCustomer(customer);
        }




        #endregion


    }
}