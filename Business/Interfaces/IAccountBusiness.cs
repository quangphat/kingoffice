using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAccountBusiness
    {

        Task<Account> Login(string username, string password);
    }
}
