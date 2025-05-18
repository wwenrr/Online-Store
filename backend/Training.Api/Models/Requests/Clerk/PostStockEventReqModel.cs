using Training.BusinessLogic.Dtos.Clerk;

namespace Training.Api.Models.Requests.Clerk {
    public class PostStockEventReqModel
    {
        public string Reason { get; set; }
        public int Quantity { get; set; }

        public StockType StockType {get; set;}
    }
}