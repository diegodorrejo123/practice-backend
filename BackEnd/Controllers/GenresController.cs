using BackEnd.Entities;
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

        public GenresController()
        {
        }
        [HttpGet]
        public ActionResult<List<Genre>> Get()
        {
            return new List<Genre>() { new Genre() { Id = 1, Name = "Comedy" } };
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Genre>> Get(int Id)
        {
            throw new NotImplementedException();
        }


        [HttpPost]
        public ActionResult Post([FromBody] Genre genre)
        {
            throw new NotImplementedException();
        }
        [HttpPut]
        public ActionResult Put([FromBody] Genre genre)
        {
            throw new NotImplementedException();
        }
        [HttpDelete]
        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
