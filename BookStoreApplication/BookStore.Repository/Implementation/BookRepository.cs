using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Domain;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository.Implementation
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Book> _books;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
            _books = _context.Set<Book>();
        }
        public List<Book> GetAllBooks()
        {
            return _books
                .Include(x => x.Publisher)
                .Include(x => x.Authors)
                .ToList();
        }
    }
}
