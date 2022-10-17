using Avtotest_WEB_FULLY.Models;

namespace Avtotest_WEB_FULLY.Service
{
    public class CookieService
    {
        public string? GetUserFromCookies(HttpContext context)
        {
            
            if (context.Request.Cookies.ContainsKey("UserPhone"))
            {
                return context.Request.Cookies["UserPhone"];
            }

            return null;
        }

        public void SendUserPhoneToCookies(string UserPhone , HttpContext context)
        {
            var options = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(2)
            };
            context.Response.Cookies.Append("UserPhone",UserPhone);
        }
        
            public void UpdateUserPhoneCookie(string userPhone, HttpContext context)
            {
                context.Response.Cookies.Delete("UserPhone");
                context.Response.Cookies.Append("UserPhone", userPhone, new CookieOptions() { Expires = DateTime.Now.AddDays(1) });
            }

        
       
    }
}
