
using JsonData.Models;
using Microsoft.AspNetCore.Mvc;

namespace Avtotest_WEB_FULLY.Controllers
{
    public class ExaminationsController : Controller
    {
        public QuestionRepository _questionRepository;

        public ExaminationsController()
        {
            _questionRepository = new QuestionRepository();
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult  GetQuestionById()
        {

        }
        //getquestionscount
        //get questionrange
    }
}
