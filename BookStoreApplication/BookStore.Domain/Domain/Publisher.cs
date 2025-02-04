using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Domain
{
    public class Publisher : BaseEntity
    {
        public string PublisherName { get; set; }
        public string Address { get; set; }  // Optional: Add more properties as needed
        public string Contact { get; set; }  // Optional: Add more properties as needed
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();  // One publisher can publish many books
    }

}
