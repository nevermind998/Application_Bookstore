using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.Model;
using LoboPraksa_Zadatak1.DAL.Interfaces;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LoboPraksa_Zadatak1.Helper;
using System.Data.SQLite;


namespace LoboPraksa_Zadatak1.DAL
{
    public class GenreDAL : IGenreDAL
    {
        private static String connectionString;
        private readonly IConfiguration _configuration;

        public GenreDAL( IConfiguration configuration)
        {
           
            _configuration = configuration;
            connectionString = ProtectionHelper.Singleton.GetSectionValue("ConnectionStrings:Connection1");
        }
        public List<Genre> GetGenres()
        {
            List<Genre> lista = new List<Genre>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM genre_table  ";

                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Genre()
                        {
                            Id = Int32.Parse(reader[0].ToString()),
                            name = reader[1].ToString()
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

        public Genre GetGenresById(long id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                Genre genre = new Genre();
               
                try
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = (@"SELECT * FROM  genre_table  WHERE id = @id");


                    SQLiteParameter parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.ParameterName = "@id";
                    parameter.Value = id;
                    command.Parameters.Add(parameter);

                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        genre = new Genre()
                        {
                            Id = Int32.Parse(reader[0].ToString()),
                            name = reader[1].ToString()
                        };
                      
                    }
                    reader.Close();
                    command.ExecuteNonQuery();
                    connection.Close();
                    return genre;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
