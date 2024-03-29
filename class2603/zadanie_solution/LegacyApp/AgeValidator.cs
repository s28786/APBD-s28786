using System;
using System.Collections.Generic;
using System.Text;

namespace LegacyApp
{
    internal class AgeValidator : IUserValidator
    {
        public AgeValidator()
        { }

        public bool Validate(User user)
        {
            if (user.DateOfBirth == null)
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - user.DateOfBirth.Year;
            if (now.Month < user.DateOfBirth.Month || (now.Month == user.DateOfBirth.Month && now.Day < user.DateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }
            return true;
        }
    }
}