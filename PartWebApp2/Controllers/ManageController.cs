using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PartWebApp2.Services;
using PartWebApp2.Models;
using System.Security.Claims;
using PartWebApp2.Data;

namespace PartWebApp2.Controllers
{
    public class ManageController : Controller
    {
        private readonly IManageService _manageService;
        private readonly PartyWebAppContext _context;

        public ManageController(IManageService manageService, PartyWebAppContext context)
        {
            _manageService = manageService;
            _context = context;
        }

        public int findCurrentUserId()
        {
            return Int32.Parse(HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        public void initTypeUserToViewData(User currentUser)
        {
            ViewData["UserFullName"] = currentUser.firstName + " " + currentUser.lastName;
            if (currentUser.Type == UserType.Admin)
            {
                ViewData["UserType"] = "Manager";
            }
            else if (currentUser.Type == UserType.producer)
            {
                ViewData["UserType"] = "Producer";
            }
            else
            {
                ViewData["UserType"] = "Client";
            }
        }
        public User returnCurrentUser()
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == findCurrentUserId());
            initTypeUserToViewData(currentUser);
            return currentUser;
        }

        public IActionResult Index()
        {
            initTypeUserToViewData(returnCurrentUser());
            return View();
        }

        public string GenreStatistics()
        {
            initTypeUserToViewData(returnCurrentUser());
            return _manageService.GetPartiesInGenre();
        }
        public string ClubStatistics()
        {
            initTypeUserToViewData(returnCurrentUser());
            return _manageService.GetPartiesInClub();
        }
        public string AreaStatistics()
        {
            initTypeUserToViewData(returnCurrentUser());
            return _manageService.GetPartiesInArea();
        }
    }
}
