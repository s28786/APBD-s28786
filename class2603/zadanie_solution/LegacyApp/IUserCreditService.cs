using LegacyApp;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegacyApp
{
    internal interface IUserCreditService
    {
        public void ConfigCreditLimit(Client client, User user)
        {
            {
                if (client.Type == "VeryImportantClient")
                {
                    user.HasCreditLimit = false;
                }
                else if (client.Type == "ImportantClient")
                {
                    using (var userCreditService = new UserCreditService())
                    {
                        int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                        creditLimit = creditLimit * 2;
                        user.CreditLimit = creditLimit;
                    }
                }
                else
                {
                    user.HasCreditLimit = true;
                    using (var userCreditService = new UserCreditService())
                    {
                        int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                        user.CreditLimit = creditLimit;
                    }
                }
            }
        }
    }
}