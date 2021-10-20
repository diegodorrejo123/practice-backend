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
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStocker fileStocker;
        private readonly string container = "authors";

        public AuthorsController(ApplicationDbContext context,
            IMapper mapper, IFileStocker fileStocker)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStocker = fileStocker;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorDTO>>> Get([FromQuery]PaginationDTO paginationDTO)
        {
            var queryable = context.Authors.AsQueryable();
            await HttpContext.InsertParametersPaginationHeader(queryable);
            var authors = await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();
            return mapper.Map<List<AuthorDTO>>(authors);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm]AuthorCreateDTO authorCreateDTO)
        {
            var author = mapper.Map<Author>(authorCreateDTO);
            if(authorCreateDTO.Photo != null)
            {
                author.Photo = await fileStocker.storeFile(container, authorCreateDTO.Photo);
            }
            context.Add(author);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("{Id:int}")]
        public async Task<ActionResult<AuthorDTO>> Get(int Id)
        {
            var author = await context.Authors.FindAsync(Id);
            if (author == null) { return NotFound(); }
            return mapper.Map<AuthorDTO>(author);
        }
        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int Id, [FromForm] AuthorCreateDTO authorCreateDTO)
        {
            var author = await context.Authors.FindAsync(Id);
            if (author == null) { return NotFound(); }
            author = mapper.Map(authorCreateDTO, author);
            if (authorCreateDTO.Photo != null)
            {
                author.Photo = await fileStocker.updateFile(container, authorCreateDTO.Photo, author.Photo);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var author = await context.Authors.FirstOrDefaultAsync(x => x.Id == Id);

            if (author == null) { return NotFound(); }
            context.Remove(author);
            await context.SaveChangesAsync();
            await fileStocker.deleteFile(author.Photo, container);
            return NoContent();
        }
    }
}
