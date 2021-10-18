using AutoMapper;
using BackEnd.DTOs;
using BackEnd.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Utility
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<GenreCreateDTO, Genre>();
            CreateMap<IdentityUser, UserDTO>();
        }
    }
}
