using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Training.DataAccess.Entities;

namespace Training.Repository.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {

    }
    public class ProductRepository(DbContext context) : BaseRepository<Product>(context), IProductRepository
    {
    }
}
