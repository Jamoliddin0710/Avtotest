using Avtotest_WEB_FULLY.Models;
using Avtotest_WEB_FULLY.Repositories;
using Avtotest_WEB_FULLY.Service;
using Microsoft.AspNetCore.Mvc;

namespace Avtotest_WEB_FULLY.Controllers
{

    public class UsersController : Controller
    {
        private readonly UsersRepository _usersRepository;
        private readonly CookieService _cookiesService;
        private readonly UserService _userService;
        private readonly TicketRepository _ticketRepository;
        private readonly QuestionRepository _questionRepository;

        public UsersController()
        {
            _usersRepository = new UsersRepository();
            _cookiesService = new CookieService();
            _userService = new UserService();
            _ticketRepository = new TicketRepository();
            _questionRepository = new QuestionRepository();
        }

        public IActionResult Index()
        {
            var user = _userService.GetUserFromCookies(HttpContext);
            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("SignIn");
            //cookieichida user phone nomeri bormi yo'qmi tekshiradi
            //agar bo'lsa shu userni oladi va actionga jo'natadi
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var currentuser = _usersRepository.GetUserByPhoneNumber(user.Phone);
            if (user.Password == currentuser.Password)
            {
                _cookiesService.SendUserPhoneToCookies(currentuser.Phone!, HttpContext);
                return RedirectToAction("Index");
            }

            return RedirectToAction("SignIn");
            // userni phone si bo'yicha oladi va passwordini tekshiradi agar phone
            // va passwordi mavjud bo'lsa unga cookie beradi va index ga yuboradi
            // aks holda user qaytadan ro'yxatdan o'tishiga to'gri keladi
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            } 
            user.Image ??= "user.png";
            _usersRepository.InsertUser(user);
            _cookiesService.SendUserPhoneToCookies(user.Phone, HttpContext);
            var _user = _usersRepository.GetUserByPhoneNumber(user.Phone);
            _ticketRepository.InsertUserTicketsTraining(_user.Index, _questionRepository.GetQuestionCount() / 20, 20);
            _cookiesService.SendUserPhoneToCookies(_user.Phone, HttpContext);
            return RedirectToAction("Index");
            //userni qo'shadi va unga cookiega qiymat beradi va index ga jo'natadi   
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit( [FromForm]Models.User user)
        {
            var _user = _userService.GetUserFromCookies(HttpContext);
            if (_user == null)
                return RedirectToAction("SignIn");


            if (!ModelState.IsValid)
                return View(user);

            _cookiesService.UpdateUserPhoneCookie(user.Phone, HttpContext);
            user.Index = _user.Index;
            user.Image = SaveImage(user.ImageFile);
            _usersRepository.UpdateUser(user);
            return RedirectToAction("Index");
        }

        private string? SaveImage(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                return "user.png";
            }

            var imagePath = Guid.NewGuid().ToString("N") + Path.GetExtension(imageFile.FileName);
            var ms = new MemoryStream();
            imageFile.CopyTo(ms);
            var filePath = Path.Combine("wwwroot","Profile",imagePath);
            System.IO.File.WriteAllBytes(filePath, ms.ToArray());
            return imagePath;
        }
    }
}

