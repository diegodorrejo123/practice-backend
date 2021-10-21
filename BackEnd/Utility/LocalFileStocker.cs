using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Utility
{
    public class LocalFileStocker : IFileStocker
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor accessor;

        public LocalFileStocker(IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            this.env = env;
            this.accessor = accessor;
        }

        public Task deleteFile(string route, string container)
        {
            if (string.IsNullOrEmpty(route))
            {
                return Task.CompletedTask;
            }
            var fileName = Path.GetFileName(route);
            var fileDirectory = Path.Combine(env.WebRootPath, container, fileName);
            if (File.Exists(fileDirectory))
            {
                File.Delete(fileDirectory);
            }
            return Task.CompletedTask;
        }

        public async Task<string> storeFile(string container, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string route = Path.Combine(folder, fileName);
            using(var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                await File.WriteAllBytesAsync(route, content);
                var currentUrl = $"{accessor.HttpContext.Request.Scheme}://{accessor.HttpContext.Request.Host}";
                var routeToDatabase = Path.Combine(currentUrl, container, fileName).Replace("\\", "/");
                return routeToDatabase;
            }
        }

        public async Task<string> updateFile(string container, IFormFile file, string route)
        {
            await deleteFile(route, container);
            return await storeFile(container, file);
        }
    }
}
