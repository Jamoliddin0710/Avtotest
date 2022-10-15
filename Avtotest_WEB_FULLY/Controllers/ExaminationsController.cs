using Avtotest_WEB_FULLY.Models;
using Avtotest_WEB_FULLY.Repositories;
using Avtotest_WEB_FULLY.Service;
using Microsoft.AspNetCore.Mvc;

namespace Avtotest_WEB_FULLY.Controllers
{
    public class ExaminationsController : Controller
    {
     
        private QuestionRepository _questionRepository;
        private UserService _userService;
        private TicketRepository _ticketRepository;
        private int TicketQuestionCount = 20;
        public ExaminationsController()
        {
            
            _userService = new UserService();
            _questionRepository = new QuestionRepository();
            _ticketRepository = new TicketRepository();
        }

        public IActionResult Exam(int ticketid  , int? questionId, int? choiceid )
        {
            var user = _userService.GetUserFromCookies(HttpContext);
           
            if (user == null)
            {
                return RedirectToAction("SignIn", "Users");
            }
            var ticket = _ticketRepository.GetTicketById(ticketid, user.Index);
            questionId = questionId ?? ticket.FromIndex;
            if (ticket.FromIndex <= questionId && ticket.FromIndex + TicketQuestionCount > questionId)
            {
                var question = _questionRepository.GetQuestionById(questionId.Value);
                ViewBag.Ticket = ticket;
                // ticketni oldin ishlaganmi tekshirish question va ticket id orqali tabledan ticket olish
                var _choiceid = (int?)null;
                var _answer = false;
                var _ticketdata = _ticketRepository.GetTicketDatByQuestionId(ticketid, questionId.Value);
                if (_ticketdata != null)
                {
                    _answer = _ticketdata.answer;
                    _choiceid = _ticketdata.choiceid;
                }
                else if (choiceid != null)
                {
                    var ticketdata = new TicketData();
                    var answer = question.Choices.First(choice => choice.id == choiceid).Answer;
                    ticketdata.ticketid = ticketid;
                    ticketdata.questionid = questionId.Value;
                    ticketdata.choiceid = choiceid.Value;
                    ticketdata.answer = answer;
                    
                    //insert qilish
                    _ticketRepository.InsertTicketData(ticketdata:ticketdata);
                  
                    _answer = answer;
                    _choiceid = ticketdata.choiceid; }
       
                if(_answer) _ticketRepository.UpdateCorrectTicket(ticket.id);
                if (_ticketRepository.GetResponseTicketCount(ticket.id) == ticket.QuestionCount)
                    return RedirectToAction("ResultPage", new { ticketid = ticket.id });
                ViewBag.choiceid = _choiceid;
                ViewBag.Answer = _answer;
                ViewBag.TicketData = _ticketRepository.GetTicketDataById(ticketid);
                return View(question);
            }

            return NotFound();
        }


        public IActionResult ResultPage(int ticketid)
        {
            var user = _userService.GetUserFromCookies(HttpContext);
            if (user == null)
            {
                return RedirectToAction("SignIn", "Users");
            }

            var ticket = _ticketRepository.GetTicketById(ticketid, user.Index);

            return View(ticket);
        }
        public IActionResult Index()
        {
            var user = _userService.GetUserFromCookies(HttpContext);
            if (user == null)
            {
                return RedirectToAction("SignIn", "Users");
            }

            var ticket = CreateRandomTicket(user);
            return View(ticket);
        }

        private Ticket CreateRandomTicket(User user)
        { 
            var ticketcount = _questionRepository.GetQuestionCount() / TicketQuestionCount;
            var rand = new Random().Next(0, ticketcount);
            var from = TicketQuestionCount * rand;
            var ticket = new Ticket(user.Index, from, TicketQuestionCount);
            _ticketRepository.InsertTicket(ticket);
            var id = _ticketRepository.GetLastrowId();
            ticket.id = id;

            return (ticket);
        }

        public IActionResult GetQuestionByIndex(int id)
        { 
            var question = _questionRepository.GetQuestionById(id);
            return View(question);
        }
    }
}
