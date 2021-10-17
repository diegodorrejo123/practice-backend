using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs
{
    public class GenreCreateDTO
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
    }
}
