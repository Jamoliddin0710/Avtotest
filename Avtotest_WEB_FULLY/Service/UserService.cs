using Avtotest_WEB_FULLY.Models;
using Avtotest_WEB_FULLY.Repositories;

namespace Avtotest_WEB_FULLY.Service
{
    public class UserService
    {
        private CookieService _cookieService;
        private UsersRepository _usersRepository;

        public UserService()
        {
            _cookieService = new CookieService();
            _usersRepository = new UsersRepository();
        }

        public User GetUserFromCookies(HttpContext context)
        {
            var userphone = _cookieService.GetUserFromCookies(context);
            if (userphone != null)
            {
                var user = _usersRepository.GetUserByPhoneNumber(userphone);
                if (user.Phone == userphone)
                {
                    return user;
                }
            }

            return null;
        }

    }
}
