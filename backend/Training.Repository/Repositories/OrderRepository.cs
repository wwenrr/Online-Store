using Microsoft.EntityFrameworkCore;
using Training.DataAccess.Entities;

namespace Training.Repository.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
}

public class OrderRepository(DbContext context)
    : BaseRepository<Order>(context), IOrderRepository
{
}
