using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using BookStore.Service.Interface;
using BookStore.Domain.Domain;
using BookStore.Domain.DTO;
using BookStore.Service.Implementation;
using Newtonsoft.Json;
using BookStore.Repository;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _authorService;
        private readonly IPublisherService _publisherService;
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context, IBookService bookService, IShoppingCartService shoppingCartService, IGenreService genreService, IAuthorService authorService, IPublisherService publisherService)
        {
            _bookService = bookService;
            _shoppingCartService = shoppingCartService;
            _genreService = genreService;
            _authorService = authorService;
            _publisherService = publisherService;
            _context = context;
        }

        // GET: Books
        public IActionResult Index()
        {
            var books = _context.Books
      .Include(b => b.Publisher)  // Eager load Publisher (if needed)
      .Include(b => b.Authors)    // Eager load Authors for each Book
      .ToList();


            return View(books); // Return books as JSON

        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.GetDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var genres = _genreService.GetAllGenres();
            var authors = _authorService.GetAllAuthors();
            var publishers = _publisherService.GetAllPublishers();

            BookDto dto = new BookDto

            {
                Book = null,
                Genres = genres,
                Authorsnn = authors,
                Publishers = publishers
            };

            return View(dto);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookDto bookDto)
        {
            Book book = bookDto.Book;
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("ModelState error: " + error.ErrorMessage);
                }
            }


            if (ModelState.IsValid)
            {
                // Assign the Publisher based on the selected Publisher ID
                var publisherFromDb = _publisherService.GetPublisherDetails(bookDto.PublisherId);
                if (publisherFromDb != null)
                {
                    book.Publisher = publisherFromDb;
                }

                // Get the first Author from the selected list of Author IDs
                var authors1 = new List<Author>();

                // Loop through each AuthorId in AuthorIds and get the Author details
                foreach (var authorId in bookDto.AuthorIds)
                {
                    var authorFromDb = _authorService.GetAuthorDetails(authorId);

                    if (authorFromDb != null)
                    {
                        authors1.Add(authorFromDb); // Add the author to the list if found
                    }
                }

                // Assign the authors to the book's Authors collection
                book.Authors = authors1;
                // Set the Book ID and save to the database
                book.Id = Guid.NewGuid();
                string json = JsonConvert.SerializeObject(book, Formatting.Indented);
                Console.WriteLine(json);
                _bookService.CreateNewBook(book);

                return RedirectToAction(nameof(Index));
            }

            // In case ModelState is invalid, return to view with the necessary data
            var genres = _genreService.GetAllGenres();
            var authors = _authorService.GetAllAuthors();
            var publishers = _publisherService.GetAllPublishers();

            var dto = new BookDto
            {
                Book = book,
                Genres = genres,
                Authorsnn = authors,
                Publishers = publishers
            };

            return View(dto);
        }

        public IActionResult AddToCart(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.GetDetailsForBook(id);

            BookInShoppingCart ps = new BookInShoppingCart();

            if (book != null)
            {
                ps.BookId = book.Id;
            }

            return View(ps);
        }

        [HttpPost]
        public IActionResult AddToCartConfirmed(BookInShoppingCart model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.AddToShoppingConfirmed(model, userId);
            var books = _context.Books
.Include(b => b.Publisher)  // Eager load Publisher (if needed)
.Include(b => b.Authors)    // Eager load Authors for each Book
.ToList();



            return View("Index", books);
        }


        // GET: Books/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.GetDetailsForBook(id);

            if (book == null)
            {
                return NotFound();
            }

            var genres = _genreService.GetAllGenres();
            var authors = _authorService.GetAllAuthors();
            var publishers = _publisherService.GetAllPublishers();

            var dto = new BookDto
            {
                Book = book,
                Genres = genres,
                Authorsnn = authors,
                Publishers = publishers,
                PublisherId = book.Publisher?.Id ?? Guid.Empty,
                AuthorIds = book.Authors?.Select(a => a.Id).ToList() ?? new List<Guid>()
            };

            return View(dto);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, BookDto bookDto)
        {
            if (id != bookDto.Book.Id)
            {
            }
            Console.WriteLine($"Id in function is {id}");
            Console.WriteLine($"Book id is {bookDto.Book.Id}");

            Book book = bookDto.Book;
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("ModelState error: " + error.ErrorMessage);
                }
            }


                if (ModelState.IsValid)
            {
                Console.Write("Valid State");

                try
                {
                    // Update Publisher based on the selected Publisher ID
                    var publisherFromDb = _publisherService.GetPublisherDetails(bookDto.PublisherId);
                    if (publisherFromDb != null)
                    {
                        book.Publisher = publisherFromDb;
                    }


                    // Update Authors based on the selected Author IDs
                    var authorsFromDb = _authorService.GetAllAuthors()
                        .Where(a => bookDto.AuthorIds.Contains(a.Id))
                        .ToList();

                    book.Authors = authorsFromDb;
                    _bookService.DeleteBook(book.Id);

                    _bookService.CreateNewBook(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // In case ModelState is invalid, re-populate the dropdowns
            var genres = _genreService.GetAllGenres();
            var authors = _authorService.GetAllAuthors();
            var publishers = _publisherService.GetAllPublishers();

            var dto = new BookDto
            {
                Book = book,
                Genres = genres,
                Authorsnn = authors,
                Publishers = publishers,
                PublisherId = bookDto.PublisherId,
                AuthorIds = bookDto.AuthorIds
            };

            return View(dto);
        }

        // GET: Books/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.GetDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _bookService.DeleteBook(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
