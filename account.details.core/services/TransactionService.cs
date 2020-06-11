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
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<Transaction> _repository;

        public TransactionService(IRepository<Transaction> repo)
        {
            _repository = repo;
        }

        public async Task<Transaction> GetTransactionById(string transactionId)
        {
            return await _repository.GetById(transactionId);
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(string accountNumber)
        {
            return await _repository.GetAllById(accountNumber);
        }
    }
}
