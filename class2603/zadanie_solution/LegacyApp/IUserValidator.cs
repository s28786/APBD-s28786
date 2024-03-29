using System;
using System.Collections.Generic;
using System.Text;

namespace LegacyApp
{
    internal interface IUserValidator
    {
        public abstract bool Validate(User user);
    }
}