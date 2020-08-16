using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.Model;
using LoboPraksa_Zadatak1.Helper;
using LoboPraksa_Zadatak1.DAL.Interfaces;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.SQLite;
using Google.Apis.Logging;
using Microsoft.Extensions.Logging;

namespace LoboPraksa_Zadatak1.DAL
{
    public class AuthorDAL : IAuthorDAL
    {
        private static String connectionString;
        private readonly IConfiguration _configuration;
        readonly ILogger<AuthorDAL> _logger;

        public AuthorDAL(IConfiguration configuration, ILogger<AuthorDAL> logger)
        {
            _logger = logger;
            _configuration = configuration;
            connectionString = ProtectionHelper.Singleton.GetSectionValue("ConnectionStrings:Connection1");
        }

        public List<Author> getAllAuthors() //SELECT
        {
            List<Author> lista = new List<Author>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM author_table";

                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Author()
                        {
                            ID = Int32.Parse(reader[0].ToString()),
                            firstName = reader[1].ToString(),
                            lastName = reader[2].ToString(),
                            age = Int32.Parse(reader[3].ToString())
                        }
                            );
                    }
                    reader.Close();
                    connection.Close();
                    return lista;

                }
                catch (Exception e)
                {
                    _logger.LogInformation("Error: Select from author_tabel");
                    throw new Exception(e.Message);
                }
            }
        }

        public Author getAllAuthorByID(long id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                Author author = new Author();
                try
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = ("SELECT * FROM  author_table WHERE id = @id");

                    SQLiteParameter parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = id;
                    parameter.ParameterName = "@id";
                    command.Parameters.Add(parameter);


                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        author = new Author()
                        {
                            ID = Int32.Parse(reader[0].ToString()),
                            firstName = reader[1].ToString(),
                            lastName = reader[2].ToString(),
                            age = Int32.Parse(reader[3].ToString())

                        };
                    }

                    reader.Close();

                    command.ExecuteNonQuery();
                    connection.Close();
                    return author;
                }



                catch (Exception e)
                {
                    _logger.LogInformation("Erorr: Search author by id");
                    throw new Exception(e.Message);
                }
            }

        }

        public void addNew(Author author) //INSERT
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = $"INSERT INTO author_table VALUES(NULL,@firstName,@lastName,@age)";

                    SQLiteParameter parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.String;
                    parameter.ParameterName = "@firstName";
                    parameter.Value = author.firstName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.String;
                    parameter.ParameterName = "@lastName";
                    parameter.Value = author.lastName;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.ParameterName = "@age";
                    parameter.Value = author.age;
                    command.Parameters.Add(parameter);

                    command.ExecuteNonQuery();
                    connection.Close();


                }
                catch (Exception e)
                {
                    _logger.LogInformation("Author isn't added");
                    throw new Exception(e.Message);
                }
            }
        }

        public void delete(long idAuthor)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = ("DELETE FROM author_table WHERE id = @id");
                    
                    
                    SQLiteParameter parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = idAuthor;
                    parameter.ParameterName = "@id";
                    command.Parameters.Add(parameter);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Author isn't deleted");
                    throw new Exception(e.Message);
                }
            }

        }


        public List<Author> searchAuthors(Filter data)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                List<Author> lista = new List<Author>();
                try
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    
                    command.CommandText = (@"SELECT * FROM  author_table WHERE ((firstName LIKE  '%"+data.name+ "%') or (lastName LIKE  '%" + data.name + "%' )  ) and  age < @age");

                    SQLiteParameter parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = data.age;
                    parameter.ParameterName = "@age";
                    command.Parameters.Add(parameter);

                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Author()
                        {
                            ID = Int32.Parse(reader[0].ToString()),
                            firstName = reader[1].ToString(),
                            lastName = reader[2].ToString(),
                            age = Int32.Parse(reader[3].ToString())
                        }
                            );
                    }
                    reader.Close();



                    command.ExecuteNonQuery();
                    connection.Close();
                    return lista;
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Searching for authors doesnt end well :(");
                    throw new Exception(e.Message);
                }
            }

        }
    }
}
