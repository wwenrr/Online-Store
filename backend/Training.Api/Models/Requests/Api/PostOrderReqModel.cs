namespace Training.Api.Models.Requests.Api;

public class PostOrderReqModel
{
    public required long ProductId { get; set; }
    public required int Quantity { get; set; }
}
