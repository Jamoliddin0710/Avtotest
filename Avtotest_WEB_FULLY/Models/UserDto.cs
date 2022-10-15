using Avtotest_WEB_FULLY.Validations;

namespace Avtotest_WEB_FULLY.Models
{
    public class UserDto
    {
     
        [Validations.Phone]
        public string? Phone { get; set; }
        [Password]
        public string? Password { get; set; }

    }
}
