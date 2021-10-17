using BackEnd.Entities;
using BackEnd.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IRepository repository;

        public GenresController(IRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public ActionResult<List<Genre>> Get()
        {
            return repository.getGenders();
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Genre>> Get(int Id)
        {
            var genre = await repository.getGenderById(Id);
            if(genre == null)
            {
                return NotFound();
            }
            return genre;
        }


        [HttpPost]
        public ActionResult Post([FromBody] Genre genre)
        {
            return NoContent();
        }
        [HttpPut]
        public ActionResult Put([FromBody] Genre genre)
        {
            return NoContent();
        }
        [HttpDelete]
        public ActionResult Delete()
        {
            return NoContent();
        }
    }
}
