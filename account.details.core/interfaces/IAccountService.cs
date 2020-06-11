using account.details.infrastructure.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace account.details.core.interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAccounts();
        Task<Account> GetAccountById(string accountNumber);
    }
}
