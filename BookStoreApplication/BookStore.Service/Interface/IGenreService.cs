using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Domain;

namespace BookStore.Service.Interface
{
    public interface IGenreService
    {
        List<Genre> GetAllGenres();
    }
}
