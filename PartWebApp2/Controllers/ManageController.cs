using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PartWebApp2.Services;


namespace PartWebApp2.Controllers
{
    public class ManageController : Controller
    {
        private readonly IManageService _manageService;

        public ManageController(IManageService manageService)
        {
            _manageService = manageService;
        }
        public IActionResult Index()
        {
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
