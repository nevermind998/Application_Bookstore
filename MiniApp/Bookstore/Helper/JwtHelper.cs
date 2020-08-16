using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.Model;
using LoboPraksa_Zadatak1.DAL.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LoboPraksa_Zadatak1.Helper
{
    public class JwtHelper
    {
        private static readonly JwtHelper singleton;
        private IConfiguration _configure;


      
        static JwtHelper()
        {
            singleton = new JwtHelper();
         
        }


        public static JwtHelper Singleton
        {
            get { return singleton; }
        }

        public void SetConfiguration(IConfiguration configuration)
        {
            singleton._configure = configuration;
        }

        public User AuthenticateUser(User check)
        {
            User user = null;
            
            if (check != null)
            {
                user = new User
                {
                    id = check.id,
                    username = check.username,
                    password = check.password,
                    role = check.role
                };

            }
            return user;
            
        }

        public Token BuildToken(User user)
        {
            Claim[] claims = new[]
            {
                new Claim (JwtRegisteredClaimNames.Sub,user.username),
                new Claim(ClaimTypes.Role, user.role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configure["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(_configure["Jwt:Issuer"], _configure["Jwt:Issuer"], claims, expires: DateTime.Now.AddMinutes(500), signingCredentials: credentials);
            Token result = new Token();
            result.token = new JwtSecurityTokenHandler().WriteToken(token);
            result.role = user.role;
            result.username = user.username;
            return result;
        }
    }
}
