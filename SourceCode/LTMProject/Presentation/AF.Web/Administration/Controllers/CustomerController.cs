using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AF.Core;
using AF.Core.Caching;
using AF.Domain.Domain.Customers;
using AF.Domain.Infrastructure;
using AF.Services.Authentication;
using AF.Services.Customers;
using AF.Services.Security;
using AF.Web.Framework.UI;
using WebGrease.Css.Extensions;

namespace AF.Admin.Controllers
{

    public partial class CustomerController : BaseAdminController
    {
        #region Fields

        private readonly IEncryptionService _encryptionService;

        private readonly IAuthenticationService _authenticationService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
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
            ICustomerRegistrationService customerRegistrationService)
        {
            this._authenticationService = authenticationService;
            this._workContext = workContext;
            this._cacheManager = cacheManager; this._encryptionService = encryptionService;
            this._customerService = customerService;
            this._webHelper = webHelper;
            this._customerRegistrationService = customerRegistrationService;
        }

        #endregion


        #region User Login

        /// <summary>
        /// 登录注销
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Logout()
        {
            _authenticationService.SignOut();
            return Redirect("/");
        }

        #endregion


    }
}