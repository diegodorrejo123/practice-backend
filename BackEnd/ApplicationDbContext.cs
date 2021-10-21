using BackEnd.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<MovieAuthors>()
                .HasKey(x => new { x.AuthorId, x.MovieId });
            builder.Entity<MovieCinemas>()
                .HasKey(x => new { x.CinemaId, x.MovieId });
            builder.Entity<MovieGenres>()
                .HasKey(x => new { x.GenreId, x.MovieId });



            base.OnModelCreating(builder);
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieAuthors> MovieAuthors { get; set; }
        public DbSet<MovieGenres> MovieGenres { get; set; }
        public DbSet<MovieCinemas> MovieCinemas { get; set; }
    }
}
