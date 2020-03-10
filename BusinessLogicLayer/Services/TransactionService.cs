using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore.Internal;
using DBTransaction = DataAccessLayer.Entities.Transaction;
using DBUser = DataAccessLayer.Entities.User;

namespace BusinessLogicLayer.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<DBTransaction> _transactionRepository;
        private readonly IRepository<DBUser> _userRepository;

        public TransactionService(
            IRepository<DBTransaction> transactionRepository, 
            IRepository<DBUser> userRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        public void CreateStart(int userId)
        {
            var dbTransaction = new DBTransaction()
            {
                SenderUserId = null,
                ReceiverUserId = userId,
                Amount = 500,
                CreatedAt = DateTime.Now
            };
            _transactionRepository.Create(dbTransaction);
        }

        public bool Create(Transaction transaction)
        {
            var result = false;
            if (IsValid(transaction) && transaction.SenderUserId > 0)
            {
                var receiverUser = _userRepository.Find(u => u.Name == transaction.ReceiverUserName).FirstOrDefault();
                if (receiverUser == null || transaction.SenderUserId == receiverUser.Id)
                {
                    throw new ConditionException("Wrong receiver");
                }

                transaction.ReceiverUserId = receiverUser.Id;
                var isSenderUserExist = _userRepository.Find(user => user.Id == transaction.SenderUserId).Any();
                if (!isSenderUserExist)
                {
                    throw new Exception("Wrong sender");
                }

                var isReceiverUserExist = _userRepository.Find(user => user.Id == transaction.ReceiverUserId).Any();
                if(!isReceiverUserExist)
                {
                    throw new ConditionException("Wrong receiver");
                }

                lock (transaction.SenderUserId.ToString())
                //lock (transaction.ReceiverUserId.ToString())
                {
                    if(IsEnoughBalance(transaction.SenderUserId, transaction.Amount))
                    {
                        var dbTransactionSender = Map(transaction);
                        _transactionRepository.Create(dbTransactionSender);
                    }
                    else
                    {
                        throw new ConditionException("The lack of ParrotWings on your account");
                    }
                }
                result = true;
            }
            return result;
        }

        public IEnumerable<UserTransaction> GetUserTransactions(int userId, int count)
        {
            return _transactionRepository
                .Find(
                    transaction => transaction.SenderUserId == userId || transaction.ReceiverUserId == userId,
                    transaction1 => transaction1.SenderUser,
                    transaction2 => transaction2.ReceiverUser)
                .OrderByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.Id)
                .Take(count)
                .ToList()
                .Select(dbTransaction => new UserTransaction()
                {
                    Type = dbTransaction.ReceiverUserId == userId ? TransactionType.Incoming : TransactionType.Outgoing,
                    CorrespondentName = dbTransaction.ReceiverUserId != userId
                        ? dbTransaction.ReceiverUser.Name
                        : dbTransaction.SenderUser != null ? dbTransaction.SenderUser.Name : string.Empty,
                    Amount = dbTransaction.Amount,
                    CreatedAt = dbTransaction.CreatedAt,
                    Balance = GetBalanceAfterApplyTransaction(userId, dbTransaction)
                });
        }

        public decimal GetUserBalance(int userId)
        {
            var userTransactions = _transactionRepository.Find(transaction => transaction.SenderUserId == userId || transaction.ReceiverUserId == userId).ToList();
            var positiveTransaction = userTransactions.Where(transaction => transaction.ReceiverUserId == userId);
            var negativeTransaction = userTransactions.Where(transaction => transaction.SenderUserId == userId);
            return positiveTransaction.Sum(transaction => transaction.Amount) - negativeTransaction.Sum(transaction => transaction.Amount);
        }

        private decimal GetBalanceAfterApplyTransaction(int userId, DBTransaction dbTransaction)
        {
            var positiveTransaction = _transactionRepository
                .Find(transaction => (transaction.CreatedAt < dbTransaction.CreatedAt 
                                     || transaction.CreatedAt == dbTransaction.CreatedAt && transaction.Id <= dbTransaction.Id)
                                     && transaction.ReceiverUserId == userId);
            var negativeTransaction = _transactionRepository
                .Find(transaction => (transaction.CreatedAt < dbTransaction.CreatedAt
                                      || transaction.CreatedAt == dbTransaction.CreatedAt && transaction.Id <= dbTransaction.Id)
                                     && transaction.SenderUserId == userId);

            return positiveTransaction.Sum(transaction => transaction.Amount) - negativeTransaction.Sum(transaction => transaction.Amount);
        }


        private bool IsValid(Transaction transaction)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(transaction);

            Validator.TryValidateObject(transaction, context, results, true);

            return !EnumerableExtensions.Any(results);
        }

        private bool IsEnoughBalance(int userId, decimal amount)
        {
            return GetUserBalance(userId) - amount > 0;
        }

        private DBTransaction Map(Transaction transaction)
        {
            var dbTransaction = AutoMapperConfiguration.Mapper.Map(transaction, new DBTransaction());
            dbTransaction.CreatedAt = DateTime.Now;
            return dbTransaction;
        }

        private Transaction Map(DBTransaction dbTransaction)
        {
            return AutoMapperConfiguration.Mapper.Map(dbTransaction, new Transaction());
        }
    }
}