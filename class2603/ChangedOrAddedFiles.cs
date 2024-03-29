using System;
using System.Collections.Generic;
using System.Threading;

namespace LegacyApp
{
    public class UserService

    {
        private readonly IEnumerable<IUserValidator> _validators;

        private readonly IUserCreditService _userCreditService;
        private readonly IClientRepository _clientRepository;

        public UserService()
        {
            _validators = new List<IUserValidator>
            {
                new EmailValidator(),
                new AgeValidator(),
                new CreditValidator()
            };
            _userCreditService = new UserCreditService();
            _clientRepository = new ClientRepository();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            var client = _clientRepository.GetByIdFromRepo(clientId);
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            foreach (var validator in _validators)
            {
                if (!validator.Validate(user))
                {
                    return false;
                }
            }

            _userCreditService.ConfigCreditLimit(client, user);

            UserDataAccess.AddUser(user);
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace LegacyApp
{
    public class UserCreditService : IDisposable, IUserCreditService
    {
        /// <summary>
        /// Simulating database
        /// </summary>
        private readonly Dictionary<string, int> _database =
            new Dictionary<string, int>()
            {
                {"Kowalski", 200},
                {"Malewski", 20000},
                {"Smith", 10000},
                {"Doe", 3000},
                {"Kwiatkowski", 1000}
            };

        public void Dispose()
        {
            //Simulating disposing of resources
        }

        /// <summary>
        /// This method is simulating contact with remote service which is used to get info about someone's credit limit
        /// </summary>
        /// <returns>Client's credit limit</returns>
        internal int GetCreditLimit(string lastName, DateTime dateOfBirth)
        {
            int randomWaitingTime = new Random().Next(3000);
            Thread.Sleep(randomWaitingTime);

            if (_database.ContainsKey(lastName))
                return _database[lastName];

            throw new ArgumentException($"Client {lastName} does not exist");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace LegacyApp
{
    public class ClientRepository : IClientRepository
    {
        /// <summary>
        /// This collection is used to simulate remote database
        /// </summary>
        public static readonly Dictionary<int, Client> Database = new Dictionary<int, Client>()
        {
            {1, new Client{ClientId = 1, Name = "Kowalski", Address = "Warszawa, Złota 12", Email = "kowalski@wp.pl", Type = "NormalClient"}},
            {2, new Client{ClientId = 2, Name = "Malewski", Address = "Warszawa, Koszykowa 86", Email = "malewski@gmail.pl", Type = "VeryImportantClient"}},
            {3, new Client{ClientId = 3, Name = "Smith", Address = "Warszawa, Kolorowa 22", Email = "smith@gmail.pl", Type = "ImportantClient"}},
            {4, new Client{ClientId = 4, Name = "Doe", Address = "Warszawa, Koszykowa 32", Email = "doe@gmail.pl", Type = "ImportantClient"}},
            {5, new Client{ClientId = 5, Name = "Kwiatkowski", Address = "Warszawa, Złota 52", Email = "kwiatkowski@wp.pl", Type = "NormalClient"}},
            {6, new Client{ClientId = 6, Name = "Andrzejewicz", Address = "Warszawa, Koszykowa 52", Email = "andrzejewicz@wp.pl", Type = "NormalClient"}}
        };

        public ClientRepository()
        {
        }

        /// <summary>
        /// Simulating fetching a client from remote database
        /// </summary>
        /// <returns>Returning client object</returns>
        internal Client GetById(int clientId)
        {
            int randomWaitTime = new Random().Next(2000);
            Thread.Sleep(randomWaitTime);

            if (Database.ContainsKey(clientId))
                return Database[clientId];

            throw new ArgumentException($"User with id {clientId} does not exist in database");
        }

        public Client GetByIdFromRepo(int clientId)
        {
            return GetById(clientId);
        }
    }
}

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

using System;
using System.Collections.Generic;
using System.Text;

namespace LegacyApp
{
    internal interface IClientRepository
    {
        public abstract Client GetByIdFromRepo(int clientId);
    }
}

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