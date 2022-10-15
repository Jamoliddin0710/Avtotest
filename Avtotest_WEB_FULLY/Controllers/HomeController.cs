using Avtotest_WEB_FULLY.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Avtotest_WEB_FULLY.Repositories;
using Avtotest_WEB_FULLY.Service;

namespace Avtotest_WEB_FULLY.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        public HomeController(ILogger<HomeController> logger)
        {
            _userService = new UserService();
            _logger = logger;
        }

        public IActionResult Index()
        {
            bool islogin = true;
            var user = _userService.GetUserFromCookies(HttpContext);
            if (user == null)
            {
                islogin = false;
            }

            ViewBag.islogin = islogin;
            return View();

            
        }

        public IActionResult Privacy()
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