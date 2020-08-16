using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.BL.Interfaces;
using LoboPraksa_Zadatak1.DAL.Interfaces;
using LoboPraksa_Zadatak1.Model;

namespace LoboPraksa_Zadatak1.BL
{
    public class BookBL : IBookBL
    {
        private readonly IBookDAL _iBookDAL;
       

        public BookBL(IBookDAL IBookDAL)
        {
            _iBookDAL = IBookDAL;
        }

        public List<Book> getAllBooks()
        {
            return _iBookDAL.getAllBooks();
        }

        public List<Book> filterByAuthor(String filter)
        {
            return _iBookDAL.filterByAuthor(filter);
        }

     

        public List<Book> filterByTitle(String filter)
        {
            return _iBookDAL.filterByTitle(filter);
        }

        public void addNew(Book book)
        {
            _iBookDAL.addNew(book);
        }

        public void updateBook(Book book)
        {
            _iBookDAL.updateBook(book);
        }
        public List<Book> GetBooksByGenreId(long id)
        {
            return _iBookDAL.GetBooksByGenreId(id);
        }

        public List<GenreWithBook> GetBooksANDGenre(List<Genre> genres)
        {
            List<GenreWithBook> lista = new List<GenreWithBook>();
            foreach(var genr in genres)
            { 
                GenreWithBook item = new GenreWithBook()
                {
                    GenreName = genr.name,
                    books = _iBookDAL.GetBooksByGenreId(genr.Id)
                };
                if (item.books.Count > 0)
                {
                    lista.Add(item);
                }
                

            }
            return lista;
        }

    }
}
