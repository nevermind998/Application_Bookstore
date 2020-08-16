using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.BL.Interfaces;
using LoboPraksa_Zadatak1.DAL.Interfaces;
using LoboPraksa_Zadatak1.Model;

namespace LoboPraksa_Zadatak1.BL
{
    public class AuthorBL : IAuthorBL
    {
        private readonly IAuthorDAL _iAuthorDAL;
      

        public AuthorBL(IAuthorDAL iAuthorDAL)
        {
            _iAuthorDAL = iAuthorDAL;
          
        }

        public List<Author> getAllAuthors()
        {
            return _iAuthorDAL.getAllAuthors();
        }

      
      
        public List<Author> searchAuthors(Filter data)
        {
            return _iAuthorDAL.searchAuthors(data);

        }

        public Author getAllAuthorByID(long id)
        {
            return _iAuthorDAL.getAllAuthorByID(id);
        }


        public void addNew(Author author)
        {
             _iAuthorDAL.addNew(author);
        }
        public void delete(long idAuthor)
        {
             _iAuthorDAL.delete(idAuthor);
        }
       
    }
}
