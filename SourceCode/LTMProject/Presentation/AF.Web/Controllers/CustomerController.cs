using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AF.Core;
using AF.Core.Caching;
using AF.Domain.Domain.Customers;
using AF.Domain.Infrastructure;
using AF.Services.Authentication;
using AF.Services.Common;
using AF.Services.Customers;
using AF.Services.Security;
using AF.Web.Models.Customer;
using AF.Web.Framework.UI;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;


namespace AF.Web.Controllers
{

    public partial class CustomerController : BasePublicController
    {
        #region Fields

        private readonly IEncryptionService _encryptionService;

        private readonly IAuthenticationService _authenticationService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWebHelper _webHelper;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="workContext">The work context.</param>
        /// <param name="cacheManager"></param>
        /// <param name="customerService">The customer service.</param>
        /// <param name="webHelper">The web helper.</param>
        /// <param name="customerRegistrationService">The customer registration service.</param>
        public CustomerController(IAuthenticationService authenticationService,
            IWorkContext workContext, ICacheManager cacheManager, IEncryptionService encryptionService,
            ICustomerService customerService,
            IWebHelper webHelper,
            ICustomerRegistrationService customerRegistrationService, IGenericAttributeService genericAttributeService)
        {
            this._authenticationService = authenticationService;
            this._workContext = workContext;
            this._cacheManager = cacheManager; this._encryptionService = encryptionService;
            this._customerService = customerService;
            this._webHelper = webHelper;
            this._customerRegistrationService = customerRegistrationService;
            _genericAttributeService = genericAttributeService;
        }

        #endregion


        #region User Login

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录请求
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var loginResult = _customerRegistrationService.ValidateCustomer(model.Username, model.Password);
            switch (loginResult)
            {
                case CustomerLoginResults.Successful:
                {
                    var customer = _customerService.GetCustomerByUsername(model.Username);

                    _authenticationService.SignIn(customer, model.RememberMe);

                    if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                        return RedirectToAction("Index", "Home");

                    return Redirect(returnUrl);
                }
                case CustomerLoginResults.CustomerNotExist:
                    ModelState.AddModelError("", "用户不存在");
                    break;
                case CustomerLoginResults.Deleted:
                    ModelState.AddModelError("", "用户已删除");
                    break;
                case CustomerLoginResults.NotActive:
                    ModelState.AddModelError("", "用户未激活");
                    break;
                case CustomerLoginResults.WrongPassword:
                    ModelState.AddModelError("", "密码错误");
                    break;
                default:
                    ModelState.AddModelError("", "登录失败");
                    break;
            }

            return View(model);
        }

        /// <summary>
        /// 登录注销
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Login");
        }

        #endregion


        #region User Theme

        [HttpPost]
        public EmptyResult Theme(int? sidebarclosed)
        {
            if (sidebarclosed.HasValue)
            {
                var customer = _workContext.CurrentCustomer;
                _genericAttributeService.SaveAttribute(customer, "sidebarclosed", sidebarclosed);
            }
          
            return new EmptyResult();
        }

        #endregion


        #region UserManager




        /// <summary>
        /// 根据条件获取用户详情
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public ActionResult ListCustomer(CustomerListModel model, CustomerListPagingFilteringModel command)
        {
            if (command.PageNumber <= 0)
            {
                command.PageNumber = 1;
            }
            if (null == model)
                model = new CustomerListModel();


            var roles = _customerService.GetAllCustomerRoles();
            model.AvailableRoles = roles.ToSelectItems();
            model.AvailableRoles.Insert(0, new SelectListItem() {Text = "请选择", Value = "0"});

            model.AvailableActive = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "请选择", Value = "0" , Selected = true},
                new SelectListItem() { Text = "激活", Value = "1" },
                new SelectListItem() { Text = "停用", Value = "2" }
            };

            bool? active = null;
            if (model.ActiveId == 1)
            {
                active = true;
            }
            else if (model.ActiveId == 2)
            {
                active = false;
            }

            var list = _customerService.GetAllCustormers(model.UserName, model.CustomerRoleId, active, model.BeginDate,
                model.EndDate,
                pageIndex: command.PageNumber - 1,
                pageSize: command.PageSize);

            model.Customers = list;
            model.PagingFilteringContext.LoadPagedList(list);
            return View(model);

        }


        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var model = new CustomerCreateModel();
            var roles = _customerService.GetAllCustomerRoles();
            model.AvailableRoles = roles.ToSelectItems();
            model.IsActive = true;
            return View(model);
        }


        /// <summary>
        /// Creates the or edit.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CustomerCreateModel model)
        {
            var roles = _customerService.GetAllCustomerRoles();
            model.AvailableRoles = roles.ToSelectItems();

            if (!ModelState.IsValid) return View(model);

            var customer = new Customer
            {
                Username = model.UserName,
                CreatedOn = DateTime.Now,
                MobilePhone = model.PhoneNumber,
                RealName = model.RealName,
                CustomerRoleId = model.CustomerRoleId,
                Active = model.IsActive,
                LastActivityDate = DateTime.Now
            };
            var customerRequest = new CustomerRegistrationRequest(customer, model.UserName, model.PassWord,
                PasswordFormat.Hashed);
            var reponse = _customerRegistrationService.RegisterCustomer(customerRequest);
            if (reponse.Success)
            {
                return RedirectToAction("ListCustomer");
            }

            reponse.Errors.ForEach(d => { ModelState.AddModelError(string.Empty, d); });
            return View(model);
        }



        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            var model = new CustomerUpdateModel();
            var roles = _customerService.GetAllCustomerRoles();
            model.AvailableRoles = roles.ToSelectItems();
            var customer = _customerService.GetCustomerById(id);
            model.UserName = customer.Username;
            model.RealName = customer.RealName;
            model.Id = id;
            model.IsActive = customer.Active;
            model.RoleId = customer.CustomerRoleId;
            model.PhoneNumber = customer.MobilePhone;
            return View(model);
        }


        /// <summary>
        /// Creates the or edit.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(CustomerUpdateModel model)
        {
            var roles = _customerService.GetAllCustomerRoles();
            model.AvailableRoles = roles.ToSelectItems();
            
            if (!ModelState.IsValid) return View(model);

            var customer =  _customerService.GetCustomerById(model.Id) ;
            customer.Username = model.UserName;
            customer.MobilePhone = model.PhoneNumber;
            if (!String.IsNullOrEmpty(model.PassWord))
            {
                string saltKey = _encryptionService.CreateSaltKey(5);
                customer.PasswordSalt = saltKey;
                customer.Password = _encryptionService.CreatePasswordHash(model.PassWord, saltKey);
            }
            customer.RealName = model.RealName;
            customer.CustomerRoleId = model.RoleId;
            customer.Active = model.IsActive;
            customer.LastActivityDate = DateTime.Now;
            _customerService.UpdateCustomer(customer);
            return RedirectToAction("ListCustomer");
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                var customer = _customerService.GetCustomerById(id);
                customer.Deleted = true;
                _customerService.UpdateCustomer(customer);
            }
            return RedirectToAction("ListCustomer");
        }


        #endregion


        #region Role Permission


        /// <summary>
        /// Permissions the role edit.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult PermissionRoleEdit(CustomerRolePermissionModel model)
        {
            var roles = _customerService.GetAllCustomerRoles();
            model.AvailableRoles = roles.ToSelectItems();
            model.PermissionRecords = _customerService.GetAllPermissionRecords();
            if (model.CustomerRoleId == 0)
            {
                var permis = roles.First();
                model.CustomerRoleId = permis.Id;
                model.RolePermissionRecords = permis.PermissionRecords.ToList();
            }
            else
            {
                model.RolePermissionRecords = roles.First(d => d.Id == model.CustomerRoleId).PermissionRecords.ToList();
            }

            return View(model);
        }



        /// <summary>
        /// Permissions the role update.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PermissionRoleUpdate(CustomerRolePermissionModel model)
        {
            var roles = _customerService.GetAllCustomerRoles();
            var permisions = _customerService.GetAllPermissionRecords();

            model.AvailableRoles = roles.ToSelectItems();
            model.PermissionRecords = permisions;
            var currentrole = roles.First(d => d.Id == model.CustomerRoleId);
            //clear old permission then add new permission
            currentrole.PermissionRecords.Clear();
            if (null != model.SelectPermission && model.SelectPermission.Count != 0)
            {
                permisions.Where(d => model.SelectPermission.Contains(d.Id)).ForEach(d =>
                {
                    currentrole.PermissionRecords.Add(d);
                });
            }

            _customerService.UpdateCustomerRole(currentrole);

            return RedirectToAction("ListCustomer", "Customer");
        }


        #endregion
    }
}