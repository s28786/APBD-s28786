using System;
using System.Collections.Generic;
using System.Text;

namespace LegacyApp
{
    internal class CreditValidator : IUserValidator
    {
        public CreditValidator()
        { }

        public bool Validate(User user)
        {
            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }
            return true;
        }
    }
}