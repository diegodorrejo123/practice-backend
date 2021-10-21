using AutoMapper;
using BackEnd.DTOs;
using BackEnd.Entities;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Utility
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<GenreCreateDTO, Genre>();
            CreateMap<IdentityUser, UserDTO>();
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<AuthorCreateDTO, Author>()
                .ForMember(x => x.Photo, options => options.Ignore());

            CreateMap<CinemaCreateDTO, Cinema>()
                .ForMember(x => x.Location, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            CreateMap<Cinema, CinemaDTO>()
                .ForMember(x => x.Latitude, dto => dto.MapFrom(field => field.Location.Y))
                .ForMember(x => x.Longitude, dto => dto.MapFrom(field => field.Location.X));


            CreateMap<MovieCreateDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MovieGenres, options => options.MapFrom(MapMovieGenres))
                .ForMember(x => x.MovieCinemas, options => options.MapFrom(MapMovieCinemas))
                .ForMember(x => x.MovieAuthors, options => options.MapFrom(MapMovieAuthors));

            CreateMap<Movie, MovieDTO>()
                .ForMember(x => x.Genres, options => options.MapFrom(MapMovieGenres))
                .ForMember(x => x.Authors, options => options.MapFrom(MapMovieAuthors))
                .ForMember(x => x.Cinemas, options => options.MapFrom(MapMovieCinemas));
        }
        private List<CinemaDTO> MapMovieCinemas(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<CinemaDTO>();

            if (movie.MovieCinemas != null)
            {
                foreach (var cinema in movie.MovieCinemas)
                {
                    result.Add(new CinemaDTO() { 
                        Id = cinema.CinemaId, 
                        Name = cinema.Cinema.Name,
                        Latitude = cinema.Cinema.Location.Y,
                        Longitude = cinema.Cinema.Location.X
                    });
                }
            }

            return result;
        }

        private List<MovieAuthorDTO> MapMovieAuthors(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<MovieAuthorDTO>();

            if (movie.MovieAuthors != null)
            {
                foreach (var movieAuthor in movie.MovieAuthors)
                {
                    result.Add(new MovieAuthorDTO() { 
                        Id = movieAuthor.AuthorId,
                        Name = movieAuthor.Author.Name,
                        Photo = movieAuthor.Author.Photo,
                        Order = movieAuthor.Order,
                        Character = movieAuthor.Character
                    });
                }
            }

            return result;
        }

        private List<GenreDTO> MapMovieGenres(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<GenreDTO>();

            if (movie.MovieGenres != null)
            {
                foreach (var genre in movie.MovieGenres)
                {
                    result.Add(new GenreDTO() { Id = genre.GenreId, Name = genre.Genre.Name });
                }
            }

            return result;
        }


        private List<MovieAuthors> MapMovieAuthors(MovieCreateDTO movieCreateDTO, Movie movie)
        {
            var result = new List<MovieAuthors>();
            if (movieCreateDTO.CinemasIds == null) { return result; }

            foreach (var author in movieCreateDTO.Authors)
            {
                result.Add(new MovieAuthors() { AuthorId = author.Id, Character = author.Character });
            }

            return result;
        }
        private List<MovieCinemas> MapMovieCinemas(MovieCreateDTO movieCreateDTO, Movie movie)
        {
            var result = new List<MovieCinemas>();
            if (movieCreateDTO.CinemasIds == null) { return result; }

            foreach (var id in movieCreateDTO.CinemasIds)
            {
                result.Add(new MovieCinemas() { CinemaId = id });
            }

            return result;
        }
        private List<MovieGenres> MapMovieGenres(MovieCreateDTO movieCreateDTO, Movie movie)
        {
            var result = new List<MovieGenres>();
            if (movieCreateDTO.GenresIds == null) { return result; }

            foreach (var id in movieCreateDTO.GenresIds)
            {
                result.Add(new MovieGenres() { GenreId = id });
            }

            return result;
        }
    }
}
