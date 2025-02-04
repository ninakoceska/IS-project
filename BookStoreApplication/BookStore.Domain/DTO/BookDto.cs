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
        public List<Genre>? Genres { get; set; }
        public Book? Book { get; set; }

        public IEnumerable<Author>? Authorsnn { get; set; } = new List<Author>();
        public IEnumerable<Publisher>? Publishers { get; set; } = new List<Publisher>();


        public List<Guid> AuthorIds { get; set; }
        public Guid PublisherId { get; set; }
    }
}
