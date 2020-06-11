using account.details.common;
using System;

namespace account.details.infrastructure.models
{
    public class Transaction
    {
        public string Id { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public DateTime ValueDate { get; set; }

        public CurrencyType Currency { get; set; }

        public double Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Description { get; set; }

        public virtual Account Account { get; set; }
    }
}
