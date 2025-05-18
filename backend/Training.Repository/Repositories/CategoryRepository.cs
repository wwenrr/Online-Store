using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Training.DataAccess.Entities;

namespace Training.Repository.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {

    }
    public class CategoryRepository(DbContext context) : BaseRepository<Category>(context), ICategoryRepository
    {
    }
}
