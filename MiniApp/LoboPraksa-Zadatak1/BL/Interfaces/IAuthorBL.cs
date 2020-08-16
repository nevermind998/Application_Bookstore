using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.Model;

namespace LoboPraksa_Zadatak1.BL.Interfaces
{
    public interface IAuthorBL
    {
        public List<Author> getAllAuthors();
       
        public Author getAllAuthorByID(long id);
        public void addNew(Author author);
        public void delete(long idAuthor);
        public List<Author> searchAuthors(Filter data);
       
    }
}
