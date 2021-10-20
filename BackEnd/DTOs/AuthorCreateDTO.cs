using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs
{
    public class AuthorCreateDTO
    {
        [Required]
        [StringLength(maximumLength: 200)]
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime BirthDay { get; set; }
        public IFormFile Photo { get; set; }
    }
}
