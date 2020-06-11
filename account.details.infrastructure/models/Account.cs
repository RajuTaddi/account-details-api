using account.details.common;
using System;
using System.Collections.Generic;

namespace account.details.infrastructure.models
{
    public class Account
    {
        public string Id { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public AccountType AccountType { get; set; }

        public CurrencyType Currency { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
