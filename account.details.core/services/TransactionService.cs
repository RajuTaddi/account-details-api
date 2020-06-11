using account.details.common.exceptions;
using account.details.core.interfaces;
using account.details.infrastructure.interfaces;
using account.details.infrastructure.models;
using account.details.infrastructure.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var transaction = await _repository.GetById(transactionId);
            if (transaction == null)
                throw new DataNotFoundException($"{transactionId} is not found");

            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(string accountNumber)
        {
            var transactions = await _repository.GetAllById(accountNumber);
            if (!transactions.Any())
                throw new DataNotFoundException($"no transaction found for account {accountNumber}");

            return transactions;
        }
    }
}
