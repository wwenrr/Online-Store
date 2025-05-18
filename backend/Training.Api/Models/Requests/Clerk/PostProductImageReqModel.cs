namespace Training.Api.Models.Requests.Api
{
    public class PostProductImageReqModel
    {
        public long? ProductID { get; set; }

        public IFormFile ImageFile { get; set; }

        public string? Path { get; set; }
    }
}
