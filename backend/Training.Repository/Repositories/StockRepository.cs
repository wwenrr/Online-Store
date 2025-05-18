using Training.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Training.Repository.Repositories
{
    public interface IStockRepository : IBaseRepository<Stock>
    {

    }
    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        public StockRepository(DbContext context) : base(context) { }
    }
}
