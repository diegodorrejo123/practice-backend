using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BackEnd.Utility
{
    public interface IFileStocker
    {
        Task deleteFile(string route, string container);
        Task<string> storeFile(string container, IFormFile file);
        Task<string> updateFile(string container, IFormFile file, string route);
    }
}