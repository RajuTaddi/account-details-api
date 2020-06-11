using account.details.infrastructure.interfaces;
using account.details.infrastructure.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace account.details.infrastructure.repositories
{
    public class AccountRepository : IRepository<Account>, IDisposable
    {
        private bool disposed;
        private readonly GlobalDbContext _context;

        public AccountRepository(GlobalDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> GetById(string accountNumber)
        {
            var accounts = await _context.Accounts
                .Where(a => a.AccountNumber == accountNumber)
                .Include(a => a.Transactions)
                .ToArrayAsync();

            var response = accounts.Select(a => new Account
            {
                AccountName = a.AccountName,
                AccountNumber = a.AccountNumber,
                AccountType = a.AccountType,
                Amount = a.Amount,
                Currency = a.Currency,
                Date = a.Date,
                Transactions = a.Transactions.Select(t => new Transaction
                {
                    AccountName = t.AccountName,
                    AccountNumber = t.AccountNumber,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    Description = t.Description,
                    ValueDate = t.ValueDate,
                    TransactionType = t.TransactionType,
                    Id = t.Id
                })
            }).FirstOrDefault();

            return response;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    _context.Dispose();
                disposed = true;
            }
        }

        public Task<IEnumerable<Account>> GetAllById(string id)
        {
            // not implemented/applicable, needs code refactor
            return null;
        }
    }
}
