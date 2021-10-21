using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs
{
    public class LandingPageDTO
    {
        public List<MovieDTO> InCinemas { get; set; }
        public List<MovieDTO> NextReleases { get; set; }
    }
}
