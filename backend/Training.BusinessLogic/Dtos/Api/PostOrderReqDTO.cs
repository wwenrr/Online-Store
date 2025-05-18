namespace Training.BusinessLogic.Dtos.Api;

public class PostOrderReqDTO
{
    public required long ProductId { get; set; }
    public required int Quantity { get; set; }

    public string? Email { get; set; }
}
