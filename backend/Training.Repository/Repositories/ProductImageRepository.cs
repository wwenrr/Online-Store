using Microsoft.EntityFrameworkCore;
using Training.DataAccess.Entities;

namespace Training.Repository.Repositories
{
    public interface IProductImageRepository : IBaseRepository<ProductImage>
    {
    }
    public class ProductImageRepository(DbContext context) : BaseRepository<ProductImage>(context), IProductImageRepository
    {
        
    }
}
