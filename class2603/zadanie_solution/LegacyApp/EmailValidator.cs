using System;
using System.Collections.Generic;
using System.Text;

namespace LegacyApp
{
    internal class EmailValidator : IUserValidator
    {
        public EmailValidator()
        { }

        public bool Validate(User user)
        {
            if (string.IsNullOrEmpty(user.EmailAddress))
            {
                return false;
            }

            if (!user.EmailAddress.Contains("@") && !user.EmailAddress.Contains("."))
            {
                return false;
            }

            return true;
        }
    }
}