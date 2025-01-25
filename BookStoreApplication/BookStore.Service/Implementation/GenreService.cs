using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Domain;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;

namespace BookStore.Service.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> _repository;

        public GenreService(IRepository<Genre> genre)
        {
            _repository = genre;
        }
        public List<Genre> GetAllGenres()
        {
            return _repository.GetAll().ToList();
        }
    }
}
