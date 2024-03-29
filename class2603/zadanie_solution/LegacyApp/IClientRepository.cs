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