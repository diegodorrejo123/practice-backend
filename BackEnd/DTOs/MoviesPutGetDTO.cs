using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs
{
    public class MoviesPutGetDTO
    {
        public MovieDTO Movie { get; set; }
        public List<GenreDTO> GenresSeleccted { get; set; }
        public List<GenreDTO> GenresNotSeleccted { get; set; }
        public List<CinemaDTO> CinemasSeleccted { get; set; }
        public List<CinemaDTO> CinemasNotSeleccted { get; set; }
        public List<MovieAuthorDTO> MovieAuthors { get; set; }
    }
}
