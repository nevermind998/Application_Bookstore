using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoboPraksa_Zadatak1.Model
{
    public class Book
    {
        public long ID { get; set; }
        public String title { get; set; }
        public long idAuthor { get; set; }
        public Author author { get; set; }
        public long numberOfPage { get; set; }
        public long genreId { get; set; }

    }
}
