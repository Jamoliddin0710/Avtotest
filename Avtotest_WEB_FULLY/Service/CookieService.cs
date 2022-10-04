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

        public void SendUserPhone(string UserPhone , HttpContext context)
        {
           context.Response.Cookies.Append("UserPhone",UserPhone);
        }
    }
}
