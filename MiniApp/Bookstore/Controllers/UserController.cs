using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoboPraksa_Zadatak1.Model;
using LoboPraksa_Zadatak1.BL.Interfaces;


namespace LoboPraksa_Zadatak1.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserBL _iUserBL;

        public UserController(IUserBL iUserBL)
        {
            _iUserBL = iUserBL;
        }

        [Route("getUser")]
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_iUserBL.SelectUser());
        }

        [Route("insertUser")]
        [HttpPost]
        public ActionResult insertUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            _iUserBL.InsertUser(user);
            return Ok();
        }

    }
}
