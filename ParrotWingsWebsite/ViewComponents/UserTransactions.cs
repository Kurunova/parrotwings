using System;
using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ParrotWingsWebsite.ViewComponents
{
    public class UserTransactions : ViewComponent
    {
        private readonly ITransactionService _transactionService;
        
        public UserTransactions(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public IViewComponentResult Invoke(ClaimsPrincipal user)
        {
            var userId = Int32.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            var userTransactions = _transactionService.GetUserTransactions(userId, 15);
            return View(userTransactions);
        }
    }
}