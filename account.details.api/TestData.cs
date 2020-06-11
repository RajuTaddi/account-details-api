using account.details.common;
using account.details.infrastructure;
using account.details.infrastructure.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace account.details.api
{
    public class TestData
    {
        internal static void LoadTestData(GlobalDbContext context)
        {
            const long accNumberBase = 1000000100;
            var random = new Random();
            string[] descriptionArray = { "Woolworths store payment", "Coles store payment", "Dominos online payment", "Online payment transfer"};

            for (int i = 1; i <= 15; i++)
            {
                var account = new Account
                {
                    AccountNumber = $"{accNumberBase + i + 10}",
                    AccountName = $"{((i % 2) == 0 ? "SG" : "AU")}{((i % 3) == 0 ? "Current" : "Savings")}{i + 10}",
                    AccountType = (i % 3) == 0 ? AccountType.Current : AccountType.Savings,
                    Amount = Math.Round(random.NextDouble() * 100000, 2),
                    Currency = (i % 2) == 0 ? CurrencyType.SGD : CurrencyType.AUD,
                    Date = DateTime.Now,
                };

                var noOfTransactions = random.Next(5, 15);
                var transactions = new List<Transaction>();
                
                for (int j = 1; j <= noOfTransactions; j++)
                {
                    var transaction = new Transaction
                    {
                        Account = account,
                        AccountName = account.AccountName,
                        AccountNumber = account.AccountNumber,
                        Amount = random.Next(10, 100),
                        Currency = (j % 2) == 0 ? CurrencyType.SGD : CurrencyType.AUD,
                        TransactionType = (j % 3) == 0 ? TransactionType.Debit : TransactionType.Credit,
                        ValueDate = DateTime.Now.AddDays(-1 * j),
                        Description = (j % 3) == 0 ? descriptionArray[random.Next(descriptionArray.Length)] : "Online Credit"
                    };
                    transactions.Add(transaction);
                    context.Transactions.Add(transaction);
                }

                account.Transactions = transactions;
                context.Accounts.Add(account);
                context.SaveChanges();
            }
        }
    }
}
