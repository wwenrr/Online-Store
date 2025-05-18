namespace Training.BusinessLogic.Dtos.Api
{
    public class GetProductResDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public float UnitPrice { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
