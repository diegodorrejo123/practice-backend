using AutoMapper;
using BackEnd.DTOs;
using BackEnd.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]CinemaCreateDTO cinemaCreateDTO)
        {
            var cinema = mapper.Map<Cinema>(cinemaCreateDTO);
            context.Add(cinema);
            await context.SaveChangesAsync();
            return NoContent();
        }



    }
}
