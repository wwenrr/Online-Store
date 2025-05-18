namespace Training.Api.Models.Requests.Clerk
{
    public class PostProductReqModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required IFormFile ThumbnailFile { get; set; }
        public required float UnitPrice { get; set; }
        public long CategoryId { get; set; }

        public string? Thumbnail { get; set; }
    }
}
