using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Domain;

namespace BookStore.Repository.Interface
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
    }
}
