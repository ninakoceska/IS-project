using System.Collections.Generic;
using System.Net.Mail;
using BookStore.Domain.Domain;
using BookStore.Domain;
using BookStore.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookStore.DomainEntities;

namespace BookStore.Repository
{
    public class ApplicationDbContext : IdentityDbContext<BookStoreUser>
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<BookInShoppingCart> BooksInShoppingCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<BookInOrder> BookInOrders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
