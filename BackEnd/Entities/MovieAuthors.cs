using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Entities
{
    public class MovieAuthors
    {
        public int MovieId { get; set; }
        public int AuthorId { get; set; }
        public Movie Movie { get; set; }
        public Author Author { get; set; }
        [StringLength(maximumLength: 100)]
        public string Character { get; set; }
        public int Order { get; set; }
    }
}
