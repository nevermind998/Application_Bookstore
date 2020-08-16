using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.Model;

namespace LoboPraksa_Zadatak1.BL.Interfaces
{
    public interface IGenreBL
    {
        public List<Genre> GetGenres();
        public Genre GetGenresById(long id);
    }
}
