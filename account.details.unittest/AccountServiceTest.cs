using account.details.common;
using account.details.core.services;
using account.details.infrastructure.interfaces;
using account.details.infrastructure.models;
using account.details.infrastructure.repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace account.details.unittest
{
    [TestClass]
    public class AccountServiceTest
    {
        private IEnumerable<Account> AccountList;
        private IEnumerable<Transaction> TransactionList;
        [TestInitialize]
        public void Startup()
        {
            var accounts = new List<Account>();
            accounts.Add(new Account
            {
                AccountName = "Test",
                AccountNumber = "1245678",
                AccountType = AccountType.Savings,
                Amount = 100,
                Currency = CurrencyType.AUD,
            });

            accounts.Add(new Account
            {
                AccountName = "Test-2",
                AccountNumber = "12456789",
                AccountType = AccountType.Current,
                Amount = 200,
                Currency = CurrencyType.SGD,
            });

            var transactions = new List<Transaction>();
            transactions.Add(new Transaction
            {
                AccountName = accounts[0].AccountName,
                AccountNumber = accounts[0].AccountNumber,
                Account = accounts[0],
                Amount = 11,
                Currency = CurrencyType.AUD,
                Description = "test",
                TransactionType = TransactionType.Credit,
                ValueDate = DateTime.Now.AddDays(-2)
            });
            transactions.Add(new Transaction
            {
                AccountName = accounts[0].AccountName,
                AccountNumber = accounts[0].AccountNumber,
                Account = accounts[0],
                Amount = 10,
                Currency = CurrencyType.AUD,
                Description = "test",
                TransactionType = TransactionType.Debit,
                ValueDate = DateTime.Now.AddDays(-1)
            });
            transactions.Add(new Transaction
            {
                AccountName = accounts[1].AccountName,
                AccountNumber = accounts[1].AccountNumber,
                Account = accounts[1],
                Amount = 11,
                Currency = CurrencyType.SGD,
                Description = "test",
                TransactionType = TransactionType.Credit,
                ValueDate = DateTime.Now.AddDays(-2)
            });
            transactions.Add(new Transaction
            {
                AccountName = accounts[1].AccountName,
                AccountNumber = accounts[1].AccountNumber,
                Account = accounts[1],
                Amount = 10,
                Currency = CurrencyType.SGD,
                Description = "test",
                TransactionType = TransactionType.Debit,
                ValueDate = DateTime.Now.AddDays(-1)
            });
            AccountList = new List<Account>(accounts);
            TransactionList = new List<Transaction>(transactions);
        }

        [TestMethod]
        public void GetAccounts_WhenAccountsExistsShouldReturnAccountList()
        {
            var mockRepository = new Mock<IRepository<Account>>();
            mockRepository.Setup(p => p.GetAll()).Returns(Task.FromResult(this.AccountList));
            var accountService = new AccountService(mockRepository.Object);

            var accounts = (List<Account>) accountService.GetAccounts().Result;
            Assert.AreEqual(accounts, this.AccountList);
            Assert.AreEqual(accounts.Count, 2);
            Assert.AreEqual(accounts[0].AccountName, "Test");
            Assert.AreEqual(accounts[0].AccountNumber, "1245678");
            Assert.AreEqual(accounts[0].AccountType, AccountType.Savings);
            Assert.AreEqual(accounts[0].Currency, CurrencyType.AUD);
            Assert.AreEqual(accounts[0].Amount, 100);
            Assert.AreEqual(accounts[0].Transactions, null);

            Assert.AreEqual(accounts[1].AccountName, "Test-2");
            Assert.AreEqual(accounts[1].AccountNumber, "12456789");
            Assert.AreEqual(accounts[1].AccountType, AccountType.Current);
            Assert.AreEqual(accounts[1].Currency, CurrencyType.SGD);
            Assert.AreEqual(accounts[1].Amount, 200);
            Assert.AreEqual(accounts[1].Transactions, null);
        }

        [TestMethod]
        public void GetAccountById_WhenAccountExistsWithIdShouldReturnAccountDetailsWithTransactions()
        {
            var accountData = this.AccountList.ToList()[0];
            var acctWithTransactions = new Account
            {
                AccountName = accountData.AccountName,
                AccountNumber = accountData.AccountNumber,
                AccountType = accountData.AccountType,
                Amount = accountData.Amount,
                Currency = accountData.Currency,
                Transactions = this.TransactionList.Where(t => t.Account == accountData)
            };

            var mockRepository = new Mock<IRepository<Account>>();
            mockRepository.Setup(p => p.GetById(It.IsAny<string>())).Returns(Task.FromResult(acctWithTransactions));
            var accountService = new AccountService(mockRepository.Object);

            var account = accountService.GetAccountById("1245678").Result;
            Assert.AreEqual(account, acctWithTransactions);
            Assert.AreEqual(account.AccountName, "Test");
            Assert.AreEqual(account.AccountNumber, "1245678");
            Assert.AreEqual(account.AccountType, AccountType.Savings);
            Assert.AreEqual(account.Currency, CurrencyType.AUD);
            Assert.AreEqual(account.Amount, 100);
            Assert.AreEqual(account.Transactions.ToList().Count, 2);
        }

        [TestMethod]
        public void GetAccounts_WhenErrorOccuredShouldRaiseAnException()
        {
            var mockRepository = new Mock<IRepository<Account>>();
            mockRepository.Setup(p => p.GetAll()).Throws(new InvalidOperationException("Random Error"));
            var accountService = new AccountService(mockRepository.Object);

            try
            {
                IEnumerable<Account> accounts = accountService.GetAccounts().Result;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.InnerException.Message, "Random Error");
            }
        }
    }
}
