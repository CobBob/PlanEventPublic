using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlanEvent.Models;

namespace PlanEvent.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(this.User.Identity.IsAuthenticated);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });


            //     https://localhost:44314/EditAct/Index
        }

        public IActionResult CustomError()
        {
            return View();


            //     https://localhost:44314/EditAct/Index
        }
    }
}
