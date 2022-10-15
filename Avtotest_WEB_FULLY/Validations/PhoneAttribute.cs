using System.ComponentModel.DataAnnotations;

namespace Avtotest_WEB_FULLY.Validations
{
    public class PhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var _value = (string)value;
            if (string.IsNullOrEmpty(_value))
            {
                ErrorMessage = "Telefon nomer kiritilmadi";
            }

            if (_value.Length < 7)
            {
                ErrorMessage = "Telefon raqami kamida 7 ta sondan iborat bo'lishi kerak!";
            }

            if (!long.TryParse(_value, out _))
            {
                ErrorMessage = "Telefon raqami faqat sonlardan iborat bo'lishi kerak!";
                return false;
            }

            return true; // notnull da true
        }
    }
}
