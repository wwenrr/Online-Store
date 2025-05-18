using Microsoft.EntityFrameworkCore;
using Training.DataAccess.Entities;

namespace Training.Repository.Repositories;

public interface IOrderDetailRepository : IBaseRepository<OrderDetail>
{
}

public class OrderDetailRepository(DbContext context)
    : BaseRepository<OrderDetail>(context), IOrderDetailRepository
{
}
