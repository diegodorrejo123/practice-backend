using BackEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs
{
    public class MovieDTO
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }
        public bool InCinemas { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public List<MovieAuthorDTO> Authors { get; set; }
        public List<GenreDTO> Genres { get; set; }
        public List<CinemaDTO> Cinemas { get; set; }
    }
}
