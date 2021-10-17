using BackEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public interface IRepository
    {
        Task<Genre> getGenderById(int Id);
        List<Genre> getGenders();
    }
}
