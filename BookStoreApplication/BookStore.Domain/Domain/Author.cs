using BookStore.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Domain
{
    public class Author : BaseEntity
    {
        public string AuthorName { get; set; }
        public string Bio { get; set; }  // Optional: Add more properties as needed
        public virtual ICollection<Book> Books { get; set; } = new List<Book>(); // One author can have many books
    }

}
