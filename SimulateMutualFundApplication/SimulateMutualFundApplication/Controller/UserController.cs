﻿using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Model.DTO;
using Newtonsoft.Json.Linq;

namespace MutualFundSimulatorApplication.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult LoginUser([FromQuery] string userEmail,[FromQuery] string password)
        {
            try
            {
                return _userService.LoginUser(userEmail, password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            try
            {
                return _userService.Register(userDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public IActionResult Register(JObject userDtoObj)
        //{
        //    var userDto = userDtoObj.ToObject<UserDto>();
        //    return _userService.Register(userDto);
        //}

        [HttpPut]
        [Route("wallet/add")]
        public IActionResult AddMoneyToWallet([FromQuery] int id,[FromQuery] decimal amount)
        {
            try
            {
                return _userService.AddMoneyToWallet(id, amount);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
