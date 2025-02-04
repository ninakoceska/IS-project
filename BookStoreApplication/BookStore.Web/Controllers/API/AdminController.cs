using BookStore.Domain.Domain;
using BookStore.Domain.DTO;
using BookStore.Domain.Identity;
using BookStore.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IBookService _bookService;
        private readonly UserManager<BookStoreUser> _userManager;
        public AdminController (IOrderService orderService, UserManager<BookStoreUser> userManager, IBookService bookService)
        {
            _orderService = orderService;
            _userManager = userManager;
            _bookService = bookService;
        }
        [HttpGet("[action]")]
        public List<Order> GetAllOrders()
        {
            return this._orderService.GetAllOrders();
        }
        [HttpPost("[action]")]
        public Order GetDetails(BaseEntity id)
        {
            return this._orderService.GetDetailsForOrder(id);
        }

        [HttpGet("[action]")]
        public IActionResult GetAllBooks()
        {
            var books = _bookService.GetBooksJson().ToList();
            return Ok(books);
        }

        [HttpPost("[action]")]
        public bool ImportAllUsers(List<UserRegistrationDto> model)
        {
            bool status = true;

            foreach (var item in model)
            {
                var userCheck = _userManager.FindByEmailAsync(item.Email).Result;

                if (userCheck == null) {
                    var user = new BookStoreUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        ShoppingCart = new ShoppingCart()
                    };

                    var result = _userManager.CreateAsync(user, item.Password).Result;
                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }
            return status;
        }
        
    }
}
