using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Training.Common.Helpers
{
    public class FileHelper
    {
        public static async Task<string> SaveImage(IFormFile req)
        {
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..", "uploads");
            var fileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "uploads", fileName);

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await req.CopyToAsync(stream);
            }

            return fileName;
        }

        public static void DeleteImage(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "uploads", fileName);
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
