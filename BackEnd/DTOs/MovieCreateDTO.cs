using BackEnd.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs
{
    public class MovieCreateDTO
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }
        [Required]
        public bool InCinemas { get; set; }
        [Required]
        [Range(typeof(DateTime), "1/1/1966", "1/1/3000", ErrorMessage = "The ReleaseDate field is required.")]
        public DateTime ReleaseDate { get; set; }
        public IFormFile Poster { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenresIds { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CinemasIds { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<AuthorMovieCreateDTO>>))]
        public List<AuthorMovieCreateDTO> Authors { get; set; }
    }
}
