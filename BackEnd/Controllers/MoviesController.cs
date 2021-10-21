using AutoMapper;
using BackEnd.DTOs;
using BackEnd.Entities;
using BackEnd.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStocker fileStocker;
        private readonly string container = "movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileStocker fileStocker)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStocker = fileStocker;
        }

        [HttpGet("postget")]
        public async Task<ActionResult<MoviesPostGetDTO>> PostGet()
        {
            var cinemas = await context.Cinemas.ToListAsync();
            var genres = await context.Genres.ToListAsync();

            var cinemasDTO = mapper.Map<List<CinemaDTO>>(cinemas);
            var genresDTO = mapper.Map<List<GenreDTO>>(genres);
            return new MoviesPostGetDTO() { Cinemas = cinemasDTO, Genres = genresDTO };
        }

        [HttpPost("searchAuthorsByName")]
        public async Task<ActionResult<List<MovieAuthorDTO>>> SearchByName([FromBody]string Name)
        {
            if (string.IsNullOrWhiteSpace(Name)) { return new List<MovieAuthorDTO>(); }

            return await context.Authors.Where(x => x.Name.Contains(Name))
                .Select(x => new MovieAuthorDTO { Id = x.Id, Name = x.Name, Photo = x.Photo })
                .Take(5).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<LandingPageDTO>> Get()
        {
            var top = 111;
            var today = DateTime.Today;

            var nextReleases = await context.Movies
               .Where(x => x.ReleaseDate > today)
               .OrderBy(x => x.ReleaseDate).Take(top)
               .ToListAsync();

            var InCinemas = await context.Movies
                .Where(x => x.InCinemas)
                .OrderBy(x => x.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var result = new LandingPageDTO();

            result.NextReleases = mapper.Map<List<MovieDTO>>(nextReleases);
            result.InCinemas = mapper.Map<List<MovieDTO>>(InCinemas);

            return result;
        }

        [HttpGet("putGet/{Id:int}")]
        public async Task<ActionResult<MoviesPutGetDTO>> PutGet(int Id)
        {
            var movieResult = await Get(Id);

            if(movieResult.Result is NotFoundResult) { return NotFound(); }

            var movie = movieResult.Value;

            var genresSelecctedIds = movie.Genres.Select(x => x.Id).ToList();
            var genresNotSeleccted = await context.Genres
                .Where(x => !genresSelecctedIds.Contains(x.Id))
                .ToListAsync();

            var cinemasSelecctedIds = movie.Cinemas.Select(x => x.Id).ToList();
            var cinemasNotSeleccted = await context.Cinemas
                .Where(x => !cinemasSelecctedIds.Contains(x.Id))
                .ToListAsync();

            var genresNotSelecctedDTO = mapper.Map<List<GenreDTO>>(genresNotSeleccted);
            var cinemasNotSelecctedDTO = mapper.Map<List<CinemaDTO>>(cinemasNotSeleccted);

            var response = new MoviesPutGetDTO();
            response.Movie = movie;
            response.GenresSeleccted = movie.Genres;
            response.GenresNotSeleccted = genresNotSelecctedDTO;
            response.CinemasSeleccted = movie.Cinemas;
            response.CinemasNotSeleccted = cinemasNotSelecctedDTO;
            response.MovieAuthors = movie.Authors;
            return response;
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int Id, [FromForm] MovieCreateDTO movieCreateDTO)
        {
            var movie = await context.Movies
                .Include(x => x.MovieAuthors)
                .Include(x => x.MovieGenres)
                .Include(x => x.MovieCinemas)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if(movie == null) { return NotFound(); }

            movie = mapper.Map(movieCreateDTO, movie);

            if(movieCreateDTO.Poster != null)
            {
                movie.Poster = await fileStocker.updateFile(container, movieCreateDTO.Poster, movie.Poster);
            }

            WriteAuthorOrder(movie);

            var result = await context.SaveChangesAsync();
            if (result == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("filter")]

        public async Task<ActionResult<List<MovieDTO>>> filter([FromQuery] MovieFilterDTO movieFilterDTO)
        {

            var movieQueryable = context.Movies.AsQueryable();
            if (!string.IsNullOrEmpty(movieFilterDTO.Title))
            {
                movieQueryable = movieQueryable.Where(x => x.Title.Contains(movieFilterDTO.Title));
            }
            if (movieFilterDTO.InCinema)
            {
                movieQueryable = movieQueryable.Where(x => x.InCinemas);
            }
            if (movieFilterDTO.NextRelease)
            {
                var today = DateTime.Today;
                movieQueryable = movieQueryable.Where(x => x.ReleaseDate > today);
            }
            if (movieFilterDTO.GenreId != 0)
            {
                movieQueryable = movieQueryable
                    .Where(x => x.MovieGenres.Select(y => y.GenreId)
                    .Contains(movieFilterDTO.GenreId));
            }
            await HttpContext.InsertParametersPaginationHeader(movieQueryable);
            var movies = await movieQueryable.Paginate(movieFilterDTO.PaginationDTO).ToListAsync();
            return mapper.Map<List<MovieDTO>>(movies);
        }


        [HttpGet("{Id:int}")]
        public async Task<ActionResult<MovieDTO>> Get(int Id)
        {
            var movie = await context.Movies
                .Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieCinemas).ThenInclude(x => x.Cinema)
                .Include(x => x.MovieAuthors).ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if(movie == null) { return NotFound(); }

            var dto = mapper.Map<MovieDTO>(movie);
            dto.Authors = dto.Authors.OrderBy(x => x.Order).ToList();
            return dto;
        }
            
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm]MovieCreateDTO movieCreateDTO)
        {
            var movie = mapper.Map<Movie>(movieCreateDTO);
            if(movieCreateDTO.ReleaseDate > DateTime.Today)
            {
                movie.InCinemas = false;
            }
            if (movieCreateDTO.Poster != null)
            {
                movie.Poster = await fileStocker.storeFile(container, movieCreateDTO.Poster);
            }

            WriteAuthorOrder(movie);
            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == Id);

            if (movie == null) { return NotFound(); }
            context.Remove(movie);
            await context.SaveChangesAsync();
            await fileStocker.deleteFile(movie.Poster, container);
            return NoContent();
        }


        private void WriteAuthorOrder(Movie movie)
        {
            if(movie.MovieAuthors != null)
            {
                for(int i = 0; i < movie.MovieAuthors.Count; i++)
                {
                    movie.MovieAuthors[i].Order = i;
                }
            }
        }
    }
}
