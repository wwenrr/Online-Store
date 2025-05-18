using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Training.Api.Models.Responses.Base;

namespace Training.Api.Controllers
{
    [ApiController]
    [Route("public")]
    public class PublicController(
        ILogger<ExampleController> logger,
        IMapper mapper) : BaseController(logger, mapper)
    {
        [HttpGet("{image_path}")]
        public async Task<IActionResult> GetImage(string image_path)
        {
            // Xác định đường dẫn file ảnh trong thư mục
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "uploads", image_path);

            // Kiểm tra xem file có tồn tại không
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Nếu file không tồn tại, trả về lỗi 404
            }

            // Trả về ảnh dưới dạng file stream
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var fileExtension = Path.GetExtension(image_path).ToLower();

            // Dựa vào định dạng của file để trả về đúng kiểu MIME
            string contentType = fileExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                _ => "application/octet-stream" // Mặc định nếu không phải loại ảnh phổ biến
            };

            return File(fileBytes, contentType); // Trả về file
        }

    }
}
