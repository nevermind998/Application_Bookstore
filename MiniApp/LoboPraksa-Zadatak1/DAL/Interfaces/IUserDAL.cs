using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.Model;

namespace LoboPraksa_Zadatak1.DAL.Interfaces
{
    public interface IUserDAL
    {
        public List<User> SelectUser();
        public void InsertUser(User user);
        public User checkUser(LoginModel loginUser);
    }
}
