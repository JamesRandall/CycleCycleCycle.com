using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;

namespace CycleCycleCycle.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Account> _accountRepository; 

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _accountRepository = unitOfWork.GetRepository<Account>();
        }

        public Account Find(string username)
        {
            return _accountRepository.All.Where(a => a.Username == username).FirstOrDefault();
        }

        public Account Find(int id)
        {
            return _accountRepository.Find(id);
        }

        public Account Create(string username)
        {
            Account account = new Account {Username = username};
            _accountRepository.Insert(account);
            _unitOfWork.Save();
            return account;
        }
    }
}