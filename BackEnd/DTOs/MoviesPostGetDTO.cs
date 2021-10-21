using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs
{
    public class MoviesPostGetDTO
    {
        public List<GenreDTO> Genres { get; set; }
        public List<CinemaDTO> Cinemas { get; set; }
    }
}
