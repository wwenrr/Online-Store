using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Training.Cms.Controllers;

[ApiController]
[Route("public")]
public class PublicController : ControllerBase
{
    [HttpGet("image/{fileName}")]
    public IActionResult GetImage(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return NotFound();

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "uploads", fileName);
        
        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        var contentType = GetContentType(fileName);

        return File(fileBytes, contentType);
    }

    private string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}
