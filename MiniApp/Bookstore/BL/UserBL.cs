using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.BL.Interfaces;
using LoboPraksa_Zadatak1.DAL.Interfaces;
using LoboPraksa_Zadatak1.Model;

namespace LoboPraksa_Zadatak1.BL
{
    public class UserBL : IUserBL
    {
        private readonly IUserDAL _iUserDAL;

        public UserBL(IUserDAL iUserDAL)
        {
            _iUserDAL = iUserDAL;
        }

        public void InsertUser(User user)
        {
            _iUserDAL.InsertUser(user);
        }

        public List<User> SelectUser()
        {
            return _iUserDAL.SelectUser();
        }

        public User checkUser(LoginModel loginUser)
        {
            return _iUserDAL.checkUser(loginUser);
        }
    }
}
