using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoboPraksa_Zadatak1.Model;
using LoboPraksa_Zadatak1.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace LoboPraksa_Zadatak1.Controllers
{

    
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class AuthorController : Controller
    {
  
        public readonly IAuthorBL _iAuthorBL;
        readonly ILogger<AuthorController> _logger;

        public AuthorController(IAuthorBL iAuthorBL, ILogger<AuthorController> logger)
        {
            _iAuthorBL = iAuthorBL;
            _logger = logger;
        }

        [Route("getAuthors")]
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_iAuthorBL.getAllAuthors());
        }

        [Route("addAuthor")]
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult addAuthor([FromBody] Author author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            _iAuthorBL.addNew(author);
            return Ok();
        }


        [Authorize(Roles = "admin")]
        [Route("deleteAuthor")]
        [HttpPost]
        public ActionResult deleteAuthor([FromBody] Filter data)
        {
            if (data == null)
            {
                return BadRequest();
            }
            _iAuthorBL.delete(data.id);
            return Ok();
        }
     

        [Route("serachByNameAndAge")]
        [HttpPost]
        public ActionResult serachByNameAndAge([FromBody] Filter data)
        {
            if (data == null)
            {
                return BadRequest();
            }

            return Ok(_iAuthorBL.searchAuthors(data));
        }
        [Authorize(Roles = "admin")]
        [Route("getAuthorById")]
        [HttpPost]
        public ActionResult getAuthorByID([FromBody] Filter data)
        {
            if (data == null)
            {
                return BadRequest();
            }
            
            return Ok(_iAuthorBL.getAllAuthorByID(data.id));
        }
    }
}
