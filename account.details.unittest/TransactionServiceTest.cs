using account.details.common;
using account.details.core.services;
using account.details.infrastructure.interfaces;
using account.details.infrastructure.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace account.details.unittest
{
    [TestClass]
    public class TransactionServiceTest
    {
        private IEnumerable<Transaction> TransactionList;
        [TestInitialize]
        public void Startup()
        {
            var accounts = new List<Account>();
            accounts.Add(new Account
            {
                AccountName = "Test",
                AccountNumber = "1245678",
            });

            accounts.Add(new Account
            {
                AccountName = "Test-2",
                AccountNumber = "12456789",
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
            TransactionList = new List<Transaction>(transactions);
        }

        [TestMethod]
        public void GetTransactionById_WhenTransactionIdExistsShouldTransactionDetails()
        {
            var transaction = new Transaction
            {
                AccountName = "Test",
                AccountNumber = "1245678",
                Account = new Account {
                    AccountName = "Test",
                    AccountNumber = "1245678",
                    AccountType = AccountType.Savings,
                },
                Amount = 11,
                Currency = CurrencyType.AUD,
                Description = "test",
                TransactionType = TransactionType.Credit,
                ValueDate = DateTime.Now.AddDays(-2)
            };

            var mockRepository = new Mock<IRepository<Transaction>>();
            mockRepository.Setup(p => p.GetById(It.IsAny<string>())).Returns(Task.FromResult(transaction));
            var transactionService = new TransactionService(mockRepository.Object);

            var transactionResponse = transactionService.GetTransactionById("13245").Result;
            Assert.AreEqual(transactionResponse, transaction);
            Assert.AreEqual(transactionResponse.AccountName, "Test");
            Assert.AreEqual(transactionResponse.AccountNumber, "1245678");
            Assert.AreEqual(transactionResponse.Account.AccountType, AccountType.Savings);
            Assert.AreEqual(transactionResponse.Currency, CurrencyType.AUD);
            Assert.AreEqual(transactionResponse.Amount, 11);
            Assert.AreEqual(transactionResponse.TransactionType, TransactionType.Credit);
        }

        [TestMethod]
        public void GetTransactionById_WhenErrorOccuredShouldRaiseAnException()
        {
            var mockRepository = new Mock<IRepository<Transaction>>();
            mockRepository.Setup(p => p.GetById(It.IsAny<string>())).Throws(new InvalidOperationException("Random Error"));
            var transactionService = new TransactionService(mockRepository.Object);

            try
            {
                var response = transactionService.GetTransactionById("13245").Result;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.InnerException.Message, "Random Error");
            }
        }
    }
}
