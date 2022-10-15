
using Avtotest_WEB_FULLY.Models;
using Avtotest_WEB_FULLY.Repositories;
using Avtotest_WEB_FULLY.Service;
using Microsoft.AspNetCore.Mvc;

namespace Avtotest_WEB_FULLY.Controllers
{
    public class TicketsController : Controller
    {
        private TicketRepository _ticketRepository;
        private UserService _userService;

        public TicketsController()
        {
            _userService = new UserService();
            _ticketRepository = new TicketRepository();
        }

        public IActionResult Index()
        {
            var user = _userService.GetUserFromCookies(HttpContext);
            if (user == null)
            {
                return RedirectToAction("SignIn", "Users");
            }

            var tickets = _ticketRepository.GetTicketsByUserId(user.Index);

            return View(tickets);
        }

        public IActionResult Statistics()
        {
            var TicketResult = _ticketRepository.MaxCorrect();
            List<string> Name = new List<string>();
            foreach (var ticket in TicketResult)
            {
                
            }

            return View(TicketResult);
        }
    }
}
