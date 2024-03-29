using System;
using System.Collections.Generic;

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