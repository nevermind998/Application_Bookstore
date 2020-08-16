using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoboPraksa_Zadatak1.Model
{
    public class User
    {
        public long id { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public String role { get; set; }
    }
}
