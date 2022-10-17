using System.ComponentModel.DataAnnotations;

namespace Avtotest_WEB_FULLY.Validations
{
    public class PasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var _value = (string)value;
            
            if (string.IsNullOrEmpty(_value))
            {
                ErrorMessage = "parol kiriting";
                return false;
            }

            if (_value.Length < 6)
            {
                ErrorMessage = "parol uzunligin kamida 6 ta belgi bo'lishi kerak";
                return false;
            }
            return true;
            // notnull and length > 6 true 
        }


    }
}
