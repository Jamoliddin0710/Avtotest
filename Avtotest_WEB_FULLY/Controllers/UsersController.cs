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

            public UsersController()
            {
                _usersRepository = new UsersRepository();
                _cookiesService = new CookieService();
            }

            public IActionResult Index()
            {
                var UserPhone = _cookiesService.GetUserFromCookies(HttpContext);
                if (UserPhone != null)
                {
                    var user = _usersRepository.GetUserByPhoneNumber(UserPhone);
                    if (UserPhone == user.Phone)
                    {
                        return View(user);
                    }
                }

                return View();


                //cookieichida user phone nomeri bormi yo'qmi tekshiradi
                //agar bo'lsa shu userni oladi va actionga jo'natadi

            }

            public IActionResult SignUp()
            {
                return View();
            }

            public IActionResult SignIn()
            {
                return View();
            }

            public IActionResult SignUpPost (User user)
            {
              _usersRepository.InsertUser(user);
              _cookiesService.SendUserPhone(user.Phone,HttpContext);
              return RedirectToAction("Index");
              //userni qo'shadi va unga cookiega qiymat beradi va index ga jo'natadi   
            }

        public IActionResult SignInPost(User user)
        {
            var currentuser = _usersRepository.GetUserByPhoneNumber(user.Phone);
            if (user.Password == currentuser.Password)
            {
                _cookiesService.SendUserPhone(currentuser.Phone!,HttpContext);
                return RedirectToAction("Index");
            }

            return RedirectToAction("SignIn");
            // userni phone si bo'yicha oladi va passwordini tekshiradi agar phone
            // va passwordi mavjud bo'lsa unga cookie beradi va index ga yuboradi
            // aks holda user qaytadan ro'yxatdan o'tishiga to'gri keladi
        }
        }
    }

