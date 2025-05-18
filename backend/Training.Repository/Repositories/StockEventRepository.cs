using Microsoft.EntityFrameworkCore;
using Training.DataAccess.Entities;

namespace Training.Repository.Repositories {
    public interface IStockEventRepository : IBaseRepository<StockEvent>
    {

    }
    public class StockEventRepository : BaseRepository<StockEvent>, IStockEventRepository
    {
        public StockEventRepository(DbContext context) : base(context) { }
        
    }
}


