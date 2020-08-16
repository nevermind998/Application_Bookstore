using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.Model;

namespace LoboPraksa_Zadatak1.BL.Interfaces
{
    public interface IBookBL
    {
        public List<Book> getAllBooks();
        public List<Book> filterByAuthor(String filter);
        public List<Book> filterByTitle(String filter);
        public void addNew(Book book);
        public void updateBook(Book book);
        public List<Book> GetBooksByGenreId(long id);
        public List<GenreWithBook> GetBooksANDGenre(List<Genre> genres);



    }
}
