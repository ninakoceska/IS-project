using BookStore.Domain.Domain;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;

        public AuthorService(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public List<Author> GetAllAuthors()
        {
            return _authorRepository.GetAll().ToList();
        }

        public Author GetAuthorDetails(Guid id)
        {
            return _authorRepository.Get(id);
        }

        public void CreateAuthor(Author author)
        {
            _authorRepository.Insert(author);
        }

        public void UpdateAuthor(Author author)
        {
            _authorRepository.Update(author);
        }

        public void DeleteAuthor(Guid id)
        {
            var author = _authorRepository.Get(id);
            _authorRepository.Delete(author);
        }
    }
}
