using System.ComponentModel.DataAnnotations;
using Avtotest_WEB_FULLY.Validations;

namespace Avtotest_WEB_FULLY.Models
{
    public class User
    {
        public int Index { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        [Validations.Phone]
        public string? Phone { get; set; }
        [Password]
        public string? Password { get; set; }
       
        
    }
}
