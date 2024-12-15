using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NotationApp.Models
{
    public static class ValidationHelper
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled);

        public static class Errors
        {
            public const string RequiredField = "This field is required";
            public const string InvalidEmail = "Please enter a valid email address";
            public const string PasswordTooShort = "Password must be at least 6 characters long";
            public const string PasswordRequirements = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character";
            public const string PasswordsDoNotMatch = "Passwords do not match";
            public const string TitleTooLong = "Title must be less than 100 characters";
            public const string InvalidDate = "Invalid date";
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return EmailRegex.IsMatch(email);
        }

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6) return false;

            // At least one uppercase letter
            bool hasUppercase = password.Any(char.IsUpper);
            // At least one lowercase letter
            bool hasLowercase = password.Any(char.IsLower);
            // At least one number
            bool hasNumber = password.Any(char.IsDigit);
            // At least one special character
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUppercase && hasLowercase && hasNumber && hasSpecial;
        }

        public static string ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return Errors.RequiredField;
            if (title.Length > 100) return Errors.TitleTooLong;
            return string.Empty;
        }

        public static string ValidateDate(DateTime? date)
        {
            if (!date.HasValue) return Errors.RequiredField;
            if (date.Value > DateTime.Now) return Errors.InvalidDate;
            return string.Empty;
        }
    }
}
