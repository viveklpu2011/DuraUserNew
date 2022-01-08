using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewDuraApp.Services.Interfaces
{
    public interface IUserService
    {
        bool LoggedIn { get; }
        Task SignOutUser();
    }
}
