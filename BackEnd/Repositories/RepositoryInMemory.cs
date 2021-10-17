using BackEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public class RepositoryInMemory: IRepository
    {
        private List<Genre> _genders;

        public RepositoryInMemory()
        {
            _genders = new List<Genre>()
            {
                new Genre(){ Id = 1, Name = "Comedy" },
                new Genre(){ Id = 2, Name = "Action" }
            };
        }
        public List<Genre> getGenders()
        {
            return _genders;
        }
        public async Task<Genre> getGenderById(int Id)
        {
            await Task.Delay(1);
            return _genders.FirstOrDefault(x => x.Id == Id);
        }
    }
}
