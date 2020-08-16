using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.BL.Interfaces;
using LoboPraksa_Zadatak1.DAL.Interfaces;
using LoboPraksa_Zadatak1.Model;

namespace LoboPraksa_Zadatak1.BL
{
    public class GenreBL : IGenreBL
    {
        private readonly IGenreDAL _iGenreDAL;

        public GenreBL(IGenreDAL iGenreDAL)
        {
            _iGenreDAL = iGenreDAL;
        }

        public List<Genre> GetGenres()
        {
            return _iGenreDAL.GetGenres();
        }

        public Genre GetGenresById(long id)
        {
            return _iGenreDAL.GetGenresById(id);
        }
    }
}
