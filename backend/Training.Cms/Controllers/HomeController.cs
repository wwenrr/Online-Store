using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Training.BusinessLogic.Dtos.Admin;
using Training.BusinessLogic.Services;
using Training.Cms.Models;
using Training.Common;
using Training.Common.Helpers;

namespace Training.Cms.Controllers
{
    public class HomeController(
        IAdminService adminService
    ) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
