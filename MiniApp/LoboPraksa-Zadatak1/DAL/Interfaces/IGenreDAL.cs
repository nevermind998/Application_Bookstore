using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.Model;

namespace LoboPraksa_Zadatak1.DAL.Interfaces
{
    public interface IGenreDAL
    {
        public List<Genre> GetGenres();
        public Genre GetGenresById(long id);
    }
}
