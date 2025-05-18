namespace Training.Api.Models.Requests.Admin
{
    public class AdminPostCategoryReqModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required IFormFile Image { get; set; }

        public string? FileName {get; set;}
    }
}
