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
    public class BookDAL : IBookDAL
    {
        private static String connectionString;
        private readonly IAuthorDAL _iAuthorDAL;
        private readonly IConfiguration _configuration;

        public BookDAL(IAuthorDAL iAuthorDAL, IConfiguration configuration)
        {
            _iAuthorDAL = iAuthorDAL;
            _configuration = configuration;
            connectionString = ProtectionHelper.Singleton.GetSectionValue("ConnectionStrings:Connection1");
        }

        public List<Book> getAllBooks()  //SELECT
        {

            List<Book> lista = new List<Book>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM book_table";

                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Book()
                        {
                            ID = Int32.Parse(reader[0].ToString()),
                            title = reader[1].ToString(),
                            idAuthor = Int32.Parse(reader[2].ToString()),
                            numberOfPage = Int32.Parse(reader[3].ToString())
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

        public List<Book> filterByAuthor(String filter)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                List<Book> lista = new List<Book>();
                try
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = (@"SELECT * FROM  book_table bk  JOIN author_table a on  bk.idAuthor = a.id and ((a.firstName LIKE '%"+filter+"%') or ( a.lastName LIKE '%"+filter+"%' ))");
                   
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        lista.Add(new Book()
                        {
                            ID = Int32.Parse(reader[0].ToString()),
                            title = reader[1].ToString(),
                            idAuthor = Int32.Parse(reader[2].ToString()),
                            numberOfPage = Int32.Parse(reader[3].ToString())
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
                    throw new Exception(e.Message);
                }
            }

        }

        public void addNew(Book book) //INSERT
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = $"INSERT INTO book_table VALUES(NULL,@title,@idAuthor,@page)";

                    SQLiteParameter parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.String;
                    parameter.ParameterName = "@title";
                    parameter.Value = book.title;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.ParameterName = "@idAuthor";
                    parameter.Value = book.idAuthor;
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.ParameterName = "@page";
                    parameter.Value = book.numberOfPage;
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

        public List<Book> GetBooksByGenreId(long id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                List<Book> lista = new List<Book>();
                try
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = (@"SELECT * FROM  book_table WHERE  genreId = @id");

                    SQLiteParameter parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = id;
                    parameter.ParameterName = "@id";
                    command.Parameters.Add(parameter);

                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Book()
                        {
                            ID = Int32.Parse(reader[0].ToString()),
                            title = reader[1].ToString(),
                            idAuthor = Int32.Parse(reader[2].ToString()),
                            numberOfPage = Int32.Parse(reader[3].ToString())
                        }
                            );
                    }
                    reader.Close();
                    command.ExecuteNonQuery();
                    connection.Close();
                    return lista.Take(4).ToList();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
        public List<Book> filterByTitle(String filter)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                List<Book> lista = new List<Book>();
                try
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = (@"SELECT * FROM  book_table WHERE  title LIKE '%" + filter + "%'");

                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Book()
                        {
                            ID = Int32.Parse(reader[0].ToString()),
                            title = reader[1].ToString(),
                            idAuthor = Int32.Parse(reader[2].ToString()),
                            numberOfPage = Int32.Parse(reader[3].ToString())
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
                    throw new Exception(e.Message);
                }
            }
        }

        public void updateBook(Book book)
        { 
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {

                try
                {
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = (@"UPDATE book_table set title = @title WHERE id = @id");

                    SQLiteParameter parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.String;
                    parameter.Value = book.title;
                    parameter.ParameterName = "@title";
                    command.Parameters.Add(parameter);

                    parameter = command.CreateParameter();
                    parameter.DbType = System.Data.DbType.Int32;
                    parameter.Value = book.ID;
                    parameter.ParameterName = "@id";
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
    }
}
