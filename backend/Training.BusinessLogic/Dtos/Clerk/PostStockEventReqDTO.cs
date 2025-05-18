namespace Training.BusinessLogic.Dtos.Clerk;

public class PostStockEventReqDTO
{
    public long StockID { get; set; }
    public string Reason { get; set; }
    public int Quantity { get; set; }

    public StockType StockType {get; set;}

    public long? ProductID { get; set; }
}

public enum StockType
{
    In = 1,
    Out = 2
}
