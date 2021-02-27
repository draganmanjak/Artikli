
using DataAccess.UserManagement.Models;
using DataAccess.UserManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.UserManagement.Interfaces
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<string>> Login(string email, string password);

        Task<ServiceResponse<ApplicationUser>> Register(ApplicationUser user, string password);

        Task<bool> UserExists(string email);


    }
}
