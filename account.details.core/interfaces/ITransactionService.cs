using account.details.infrastructure.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace account.details.core.interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactions(string accountNumber);
        Task<Transaction> GetTransactionById(string transactionId);
    }
}
