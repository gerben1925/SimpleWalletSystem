using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SimpleWalletSystem.Model;

namespace SimpleWalletSystem.Services
{
    public interface IAuth
    {
        bool ValidateUser(string Username);
        DataSet NewUser(RegisterUser user);
    }
}
