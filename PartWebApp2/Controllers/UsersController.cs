using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartWebApp2.Data;
using PartWebApp2.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace PartWebApp2.Controllers
{
    public class UsersController : Controller
    {
        private readonly PartyWebAppContext _context;

        public UsersController(PartyWebAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        private async void Signin(User account)
        {
            var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.email),
                new Claim(ClaimTypes.Role, account.Type.ToString())
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult chooseRegisterType()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,email,password")] User user)
        {
            var foundUser = from u in _context.User
                            where u.email == user.email && u.password == user.password
                            select u;

            if (foundUser.Count() != 0)
            {
                await _context.SaveChangesAsync();
                var connectedUser = _context.User.FirstOrDefault(u => u.email == user.email && u.password == user.password);
                Signin(connectedUser);
                ViewData["Full Name"] = connectedUser.firstName + " " + connectedUser.lastName;
                return RedirectToAction("Index", "Parties");
            }
            else
            {
                ViewData["Error"] = "Username and/or password are incorrect.";
            }
            return View(user);
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,firstName,lastName,email,birthDate,password")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _context.User.FirstOrDefault(u => u.email == user.email); //if we dont have the email in the DB - return null

                if (q == null)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    var u = _context.User.FirstOrDefault(u => u.email == user.email && u.password == user.password);
                    Signin(u);

                    return RedirectToAction("Index", "Parties");
                }
                else
                {

                    ViewData["Error"] = "Unable to comply; cannot register this user.";

                }
            }
            return View(user);
        }
        public IActionResult producerRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> producerRegister([Bind("Id,firstName,lastName,email,birthDate,password")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _context.User.FirstOrDefault(u => u.email == user.email); 

                if (q == null)
                {
                    user.Type = UserType.producer;
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    var u = _context.User.FirstOrDefault(u => u.email == user.email && u.password == user.password);
                    Signin(u);

                    return RedirectToAction("Index", "Parties");
                }
                else
                {

                    ViewData["Error"] = "Unable to comply; cannot register this user.";

                }
            }
            return View(user);
        }
    }
}