using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Entities
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime BirthDay { get; set; }
        public string Photo { get; set; }
    }
}
