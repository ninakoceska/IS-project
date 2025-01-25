using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Domain;

namespace BookStore.Domain.DTO
{
    public class BookDto
    {
        public List<Genre> Genres { get; set; }
        public Book? Book { get; set; }
    }
}
