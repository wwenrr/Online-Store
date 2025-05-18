namespace Training.Cms.Models;

public class PostCategoryReqModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; }

    public string? FileName { get; set; }
}
