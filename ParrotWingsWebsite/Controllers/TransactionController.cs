using System;
using System.Security.Claims;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ParrotWingsWebsite.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            return View();
        }

        [HttpGet]
        public IActionResult CreateTransaction()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTransaction(Transaction transaction)
        {
            try
            {
                var userId = Int32.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                transaction.SenderUserId = userId;
                _transactionService.Create(transaction);
            }
            catch (ConditionException exception)
            {
                ModelState.AddModelError("General", exception.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("General", "An error occured");
            }

            return PartialView();
        }

        public IActionResult UserTransactions()
        {
            return ViewComponent("UserTransactions", new { user = User });
        }
    }
}