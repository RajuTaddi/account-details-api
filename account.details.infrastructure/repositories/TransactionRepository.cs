using account.details.infrastructure.interfaces;
using account.details.infrastructure.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace account.details.infrastructure.repositories
{
    public class TransactionRepository : IRepository<Transaction>, IDisposable
    {
        private bool disposed;
        private readonly GlobalDbContext _context;

        public TransactionRepository(GlobalDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Transaction>> GetAllById(string accountNumber)
        {
            return await _context.Transactions.Where(a => a.Account.AccountNumber == accountNumber).ToListAsync();
        }

        public async Task<Transaction> GetById(string transactionId)
        {
            return await _context.Transactions.Where(a => a.Id == transactionId).FirstOrDefaultAsync();
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

        public Task<IEnumerable<Transaction>> GetAll()
        {
            // not implemented/applicable, needs code refactor
            return null;
        }
    }
}
