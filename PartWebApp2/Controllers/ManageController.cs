using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PartWebApp2.Services;
using PartWebApp2.Models;
using PartWebApp2.Data;
using Microsoft.AspNetCore.Authorization;

namespace PartWebApp2.Controllers
{
    public class ManageController : Controller
    {
        private readonly IManageService _manageService;
        private readonly PartyWebAppContext _context;

        public ManageController(IManageService manageService, PartyWebAppContext context)
        { 
            _manageService = manageService;
            _context= context;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewData["PageName"] = "Index";
            //initTypeUserToViewData(returnCurrentUser()); - איך לגשת לזה
            return View();
        }

        public string GenreStatistics()
        {
            return _manageService.GetPartiesInGenre();
        }
        public string ClubStatistics()
        {
            return _manageService.GetPartiesInClub();
        }
        public string AreaStatistics()
        {
            return _manageService.GetPartiesInArea();
        }
    }
}
