using System;
using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ParrotWingsWebsite.ViewComponents
{
    public class UserBalance : ViewComponent
    {
        private ITransactionService _transactionService;
        
        public UserBalance(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public string Invoke(ClaimsPrincipal user)
        {
            var userId = Int32.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            var balance = _transactionService.GetUserBalance(userId);
            return balance.ToString();
        }
    }
}