using System.Collections.Generic;
using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface ITransactionService
    {
        void CreateStart(int userId);

        bool Create(Transaction transaction);

        decimal GetUserBalance(int userId);

        IEnumerable<UserTransaction> GetUserTransactions(int userId, int count);
    }
}