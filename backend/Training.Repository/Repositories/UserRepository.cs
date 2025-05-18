using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Training.DataAccess.Entities;

namespace Training.Repository.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserByEmail(string email);
    }

    public class UserRepository(DbContext context)
        : BaseRepository<User>(context), IUserRepository
    {
        public async Task<User?> GetUserByEmail(string email)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
