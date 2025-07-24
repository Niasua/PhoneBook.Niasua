using System.Text.RegularExpressions;

namespace PhoneBook.Validator;

public static class Validator
{
    public static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    public static bool IsValidPhone(string phone)
    {
        return Regex.IsMatch(phone, @"^[\d\s\-\+]+$");
    }
}
