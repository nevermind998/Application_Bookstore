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
  //  [Authorize]
    public class BookController : Controller
    {
        public readonly IBookBL _iBookBL;
        public readonly IAuthorBL _iAuthorBL;
        public readonly IGenreBL _iGenreBL;
        readonly ILogger<BookController> _logg;

        public BookController(IBookBL iBookBL, IAuthorBL iAuthorBL, IGenreBL iGenreBL, ILogger<BookController> logg)
        {
            _iBookBL = iBookBL;
            _iAuthorBL = iAuthorBL;
            _iGenreBL = iGenreBL;
            _logg = logg;
        }



        [Route("getBooks")]
        [HttpPost]
        public ActionResult Get()
        {
           return Ok(_iBookBL.getAllBooks());
        }

        [Route("getBooksByGenre")]
        [HttpGet]
        public ActionResult getBooksByGenre()
        {
            List<Genre> genres = _iGenreBL.GetGenres();
            if (genres == null)
            {
                _logg.LogInformation("No genres in list!");
            }
            return Ok(_iBookBL.GetBooksANDGenre(genres));
        }

        [Route("searchByAuthor")]
        [HttpPost]
        public ActionResult SearchByAuthor([FromBody] Filter search)
        {
            return Ok(_iBookBL.filterByAuthor(search.filter));
        }

        [Route("searchByTitle")]
        [HttpPost]
        public ActionResult SearchByTitle([FromBody] Filter search)
        {
            return Ok(_iBookBL.filterByTitle(search.filter));
        }

        [Authorize(Roles = "admin")]
        [Route("addNew")]
        [HttpPost]
        public ActionResult addNew([FromBody] Book book)
        {
            if (book == null)
            {
                _logg.LogInformation("Data that is sent is null!");
                return BadRequest();
            }

            _iBookBL.addNew(book);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [Route("updateTitleBook")]
        [HttpPost]
        public ActionResult updateTitleBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            _iBookBL.updateBook(book);
            return Ok();
        }


    }
}
