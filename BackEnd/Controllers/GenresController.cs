using AutoMapper;
using BackEnd.DTOs;
using BackEnd.Entities;
using BackEnd.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/genres")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public GenresController(ApplicationDbContext context, 
            IMapper mapper, 
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GenreDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Genres.AsQueryable();
            await HttpContext.InsertParametersPaginationHeader(queryable);
            var genres = await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();
            return mapper.Map<List<GenreDTO>>(genres);
        }

        [HttpGet("{Id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<GenreDTO>> Get(int Id)
        {
            var genre = await context.Genres.FindAsync(Id);
            if(genre == null) { return NotFound(); }
            return mapper.Map<GenreDTO>(genre);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreateDTO genreCreateDTO)
        {
            var genre = mapper.Map<Genre>(genreCreateDTO);
            context.Add(genre);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] GenreCreateDTO genreCreateDTO)
        {
            var genre = await context.Genres.FindAsync(Id);
            if (genre == null) { return NotFound(); }
            genre = mapper.Map(genreCreateDTO, genre);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var exists = await context.Genres.AnyAsync(x => x.Id == Id);
            if (!exists) { return NotFound(); }
            context.Remove(new Genre() { Id = Id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
