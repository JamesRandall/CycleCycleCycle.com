using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services;

namespace CycleCycleCycle.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IAccountService _accountService;

        protected BaseController(IAccountService accountService) : base()
        {
            _accountService = accountService;
        }

        private Account _account;
        private bool _searchedForAccount = false;
        public Account Account
        {
            get
            {
                if (!_searchedForAccount)
                {
                    if (User != null && User.Identity.IsAuthenticated)
                    {
                        _account = _accountService.Find(User.Identity.Name);
                        _searchedForAccount = true;
                    }
                }
                return _account;
            }
            internal set
            {
                _account = value;
                _searchedForAccount = true;
            }
        }
    }
}