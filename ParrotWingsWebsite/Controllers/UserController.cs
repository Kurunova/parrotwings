using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ParrotWingsWebsite.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(User user)
        {
            bool isRegistered = false;

            try
            {
                isRegistered = _userService.Register(user);
            }
            catch (ConditionException exception)
            {
                ModelState.AddModelError("General", exception.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("General", "An error occured");
            }
            
            if(isRegistered)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties{};

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Transaction");
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
            }
            else if (!_userService.CanUserSignIn(model.Email, model.Password))
            {
                ModelState.AddModelError("General", "Incorrect login or password");
            }
            else
            {
                var user = _userService.GetUser(model.Email, model.Password);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { };

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Transaction");
            }

            return View();
        }

        [Authorize]
        public ActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Transaction");
        }

        public IActionResult UserBalance()
        {
            return ViewComponent("UserBalance", new { user = User });
        }

        //[Authorize]
        public ActionResult GetUserByPartOfName(string partOfName)
        {
            var userName = this.User.FindFirstValue(ClaimTypes.Name);
            var users = _userService.GetUserByPartOfName(partOfName, userName).ToList();
            return Json(users.Select(p => new { label = p.Name }));
        }

        public ActionResult GetUserTop(int count)
        {
            var userName = this.User.FindFirstValue(ClaimTypes.Name);
            var users = _userService.GetUserTop(count, userName).Select(p => p.Name).ToList();
            return Json(users);
        }
    }
}