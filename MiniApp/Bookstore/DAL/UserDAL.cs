using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.Model;
using LoboPraksa_Zadatak1.DAL.Interfaces;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LoboPraksa_Zadatak1.Helper;

namespace LoboPraksa_Zadatak1.DAL
{
    public class UserDAL : IUserDAL
    {
        private static String connectionString;
        private readonly IConfiguration _configuration;

        public UserDAL(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = ProtectionHelper.Singleton.GetSectionValue("ConnectionStrings:Connection2");
        }

        public void InsertUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"INSERT INTO users_table VALUES(@username,@password,@role)";

                    SqlParameter parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.String;
                    parameter.ParameterName = "@username";
                    parameter.Value = user.username;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.String;
                    parameter.ParameterName = "@password";
                    parameter.Value = user.password;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.String;
                    parameter.ParameterName = "@role";
                    parameter.Value = user.role;
                    command.Parameters.Add(parameter);

                    command.ExecuteNonQuery();
                    connection.Close();


                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public List<User> SelectUser()
        {
            List<User> lista = new List<User>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM users_table";

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new User()
                        {
                            id = Int32.Parse(reader[0].ToString()),
                            username = reader[1].ToString(),
                            password = reader[2].ToString(),
                            role = reader[3].ToString(),
                        }
                            );
                    }
                    reader.Close();
                    connection.Close();
                    return lista;

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }


        public User checkUser(LoginModel loginUser)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                User user = null;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("checkUser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = loginUser.username;
                    parameter.ParameterName = "@username";
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = loginUser.password;
                    parameter.ParameterName = "@password";
                    command.Parameters.Add(parameter);



                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        user = new User()
                        {
                            id = Int32.Parse(reader[0].ToString()),
                            username = reader[1].ToString(),
                            password = reader[2].ToString(),
                            role = reader[3].ToString()
                        };
                    }
                      
                    reader.Close();
                    command.ExecuteNonQuery();
                    connection.Close();
                    return user;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
