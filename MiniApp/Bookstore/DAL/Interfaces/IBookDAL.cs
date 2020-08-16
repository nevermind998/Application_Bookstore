using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.Model;

namespace LoboPraksa_Zadatak1.DAL.Interfaces
{
    public interface IBookDAL
    {
        public List<Book> getAllBooks();
     
        public List<Book> filterByAuthor(String filter);
        public void addNew(Book book);
        public List<Book> filterByTitle(String filter);
        public List<Book> GetBooksByGenreId(long id);
        public void updateBook(Book book);



    }
}
