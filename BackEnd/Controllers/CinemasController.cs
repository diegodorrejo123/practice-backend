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
    [Route("api/cinemas")]
    [ApiController]
    public class CinemasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CinemasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CinemaDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Cinemas.AsQueryable();
            await HttpContext.InsertParametersPaginationHeader(queryable);
            var cinemas = await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();
            return mapper.Map<List<CinemaDTO>>(cinemas);
        }


        [HttpGet("{Id:int}")]
        public async Task<ActionResult<CinemaDTO>> Get(int Id)
        {
            var cinemas = await context.Cinemas.FindAsync(Id);
            if (cinemas == null) { return NotFound(); }
            return mapper.Map<CinemaDTO>(cinemas);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]CinemaCreateDTO cinemaCreateDTO)
        {
            var cinema = mapper.Map<Cinema>(cinemaCreateDTO);
            context.Add(cinema);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] CinemaCreateDTO cinemaCreateDTO)
        {
            var cinema = await context.Cinemas.FindAsync(Id);
            if (cinema == null) { return NotFound(); }
            cinema = mapper.Map(cinemaCreateDTO, cinema);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var exists = await context.Cinemas.AnyAsync(x => x.Id == Id);
            if (!exists) { return NotFound(); }
            context.Remove(new Cinema() { Id = Id });
            await context.SaveChangesAsync();
            return NoContent();
        }


    }
}
