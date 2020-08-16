using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LoboPraksa_Zadatak1.Helper;
using LoboPraksa_Zadatak1.Model;
using Microsoft.AspNetCore.Authorization;
using LoboPraksa_Zadatak1.BL.Interfaces;



namespace LoboPraksa_Zadatak1.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserBL _iUserBL;

        public LoginController(IConfiguration configuration, IUserBL iUserBL)
        {
            _configuration = configuration;
            _iUserBL = iUserBL;
            JwtHelper.Singleton.SetConfiguration(configuration);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("loginUser")]
        public IActionResult CreateToken([FromBody] LoginModel loginModel)
        {
            IActionResult response = Unauthorized();
            User check  = _iUserBL.checkUser(loginModel);
            User user = JwtHelper.Singleton.AuthenticateUser(check);

            if (user != null)
            {
                var token = JwtHelper.Singleton.BuildToken(user);
                response = Ok(token);
            }

            return response;
        }

    }
}
