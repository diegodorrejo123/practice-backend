using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Entities
{
    public class MovieGenres
    {
        public int MovieId { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public Movie Movie { get; set; }
    }
}
