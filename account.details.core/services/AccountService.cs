using account.details.common.exceptions;
using account.details.core.interfaces;
using account.details.infrastructure.interfaces;
using account.details.infrastructure.models;
using account.details.infrastructure.repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace account.details.core.services
{
    public class AccountService : IAccountService
    {
        public readonly IRepository<Account> _repository;

        public AccountService(IRepository<Account> repo)
        {
            this._repository = repo;
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _repository.GetAll();
        }

        public async Task<Account> GetAccountById(string accountNumber)
        {
            var account = await _repository.GetById(accountNumber);
            if (account == null)
                throw new DataNotFoundException($"{accountNumber} is not found");

            return account;
        }
    }
}
