using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace APIs.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly string key;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
            key = _configuration.GetValue<string>("AppSettings:Key");

        }

        [HttpPost("Authenticate")]
        public async Task<UserDto> AuthenticateUser(UserDto dto) => await _userService.AuthenticateUser(dto, key);
        [HttpPost("AddUser")]
        
        public async Task<long> PostUser(AddUserDto userDto) => await _userService.PostUser(userDto);

        [HttpPost("ChangePassword")]
        public async Task<bool> ChangeUserPassword(ChangePasswordDto changePasswordDto) => await _userService.ChangePassword(changePasswordDto);

        [HttpGet("UserProfile")]
        public async Task<GetUserProfileDto> GetUserProfile(long userId) => await _userService.GetUserProfile(userId);
        [HttpPut("[action]")]
        public async Task<ResponseModel> ProfileUpdate(UpdateUserProfileDto dto) => await _userService.ProfileUpdate(dto);
        
    }
}
