using DataLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserService
    {
        Task<UserDto> AuthenticateUser(UserDto dto, string injectkey);
    }
}
