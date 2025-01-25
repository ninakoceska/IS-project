using BookStore.DomainEntities;
using BookStore.Domain.Domain;
using BookStore.Domain.DTO;
using BookStore.Repository.Implementation;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<BookInShoppingCart> _bookInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<BookInOrder> _bookInOrder;
        private readonly IEmailService _emailService;


        public ShoppingCartService (IRepository<BookInOrder> bookInOrder, IRepository<Order> _orderRepository, IUserRepository userRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<BookInShoppingCart> bookInCart, IEmailService emailService)
        {
            this._bookInOrder = bookInOrder;
            this._orderRepository = _orderRepository;
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _bookInShoppingCartRepository = bookInCart;
            _emailService = emailService;
        }
        public bool AddToShoppingConfirmed(BookInShoppingCart model, string userId)
        {

            var loggedInUser = _userRepository.Get(userId);

            var userShoppingCart = loggedInUser.ShoppingCart;

            if (userShoppingCart.BookInShoppingCart == null)
                userShoppingCart.BookInShoppingCart = new List<BookInShoppingCart>(); ;

            userShoppingCart.BookInShoppingCart.Add(model);
            _shoppingCartRepository.Update(userShoppingCart);
            return true;
        }

        public bool deleteBookFromShoppingCart(string userId, Guid bookId)
        {
            if (bookId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                var userShoppingCart = loggedInUser.ShoppingCart;
                var book = userShoppingCart.BookInShoppingCart.Where(x => x.BookId == bookId).FirstOrDefault();

                userShoppingCart.BookInShoppingCart.Remove(book);

                _shoppingCartRepository.Update(userShoppingCart);
                return true;
            }
            return false;

        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            var userShoppingCart = loggedInUser?.ShoppingCart;
            var allBooks = userShoppingCart?.BookInShoppingCart?.ToList();

            var totalPrice = allBooks.Select(x => (x.Book.Price * x.Quantity)).Sum();
            if(totalPrice == null)
            {
                totalPrice = 0;
            }
            ShoppingCartDto dto = new ShoppingCartDto
            {
                Books = allBooks,
                TotalPrice = totalPrice
            };
            return dto;
        }

        public bool order(string userId)
        {
            if (userId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                var userShoppingCart = loggedInUser.ShoppingCart;
                EmailMessage message = new EmailMessage();
                message.Subject = "Successfull order";
                message.MailTo = loggedInUser.Email;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    userId = userId,
                    Owner = loggedInUser
                };

                _orderRepository.Insert(order);

                List<BookInOrder> bookInOrder = new List<BookInOrder>();

                var lista = userShoppingCart.BookInShoppingCart.Select(
                    x => new BookInOrder
                    {
                        Id = Guid.NewGuid(),
                        BookId = x.Book.Id,
                        Book = x.Book,
                        OrderId = order.Id,
                        Order = order,
                        Quantity = x.Quantity
                    }
                    ).ToList();


                StringBuilder sb = new StringBuilder();

                var totalPrice = 0.0;

                sb.AppendLine("Your order is completed. The order conatins: ");

                for (int i = 1; i <= lista.Count(); i++)
                {
                    var currentItem = lista[i - 1];
                    totalPrice += currentItem.Quantity * currentItem.Book.Price;
                    sb.AppendLine(i.ToString() + ". " + currentItem.Book.BookName + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.Book.Price);
                }

                sb.AppendLine("Total price for your order: " + totalPrice.ToString());
                message.Content = sb.ToString();

                bookInOrder.AddRange(lista);

                foreach (var product in bookInOrder)
                {
                    _bookInOrder.Insert(product);
                }

                loggedInUser.ShoppingCart.BookInShoppingCart.Clear();
                _userRepository.Update(loggedInUser);
                this._emailService.SendEmailAsync(message);

                return true;
            }
            return false;
        }
        
    }
}

