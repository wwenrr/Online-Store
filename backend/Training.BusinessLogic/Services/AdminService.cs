using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MySqlConnector;
using Training.BusinessLogic.Dtos.Admin;
using Training.Common.Helpers;
using Training.CustomException;
using Training.DataAccess.Entities;
using Training.Repository.UoW;
using static System.Net.Mime.MediaTypeNames;

namespace Training.BusinessLogic.Services
{   
    public interface IAdminService
    {
        Task<object> GetUser(int page);
        Task PatchUser(AdminPatchUserReqDTO reqDTO);
        Task DeleteUser(long id);

        Task PostCategory(AdminPostCategoryReqDTO reqDTO);
        Task<Category> DeleteCategory(long id);
        Task<object> GetCategory(int page);
        Task PostUser(AdminPostUserReqDTO reqDTO);

        Task RestoreCategory(long id);
        Task<object> GetLowStockProducts(int page);
        Task<object> GetTodayOrders();
        Task<object> GetHighestOrders(int page);
    }
    public class AdminService(
        IMapper mapper,
        IUnitOfWork unitOfWork) : IAdminService
    {
        public async Task<object> GetUser(int page)
        {
            var repo = unitOfWork.GetRepository<User>();

            var skipCount = (page-1) * 10;

            string sql = "SELECT * FROM User WHERE IsDeleted = 0 LIMIT 10 OFFSET @skipCount";
            var query = await repo.QueryRaw(sql, new MySqlParameter("@skipCount", skipCount));
            var users = await query.ToListAsync();

            var total = await repo.Count(u => u.IsDeleted == false);

            List<AdminGetUserResDTO> listResDTO = new List<AdminGetUserResDTO>();

            foreach (var item in users)
            {
                listResDTO.Add(mapper.Map<AdminGetUserResDTO>(item));
            }

            var resDTO = new
            {
                data = listResDTO,
                page= page,
                totalPage= Math.Ceiling((double)total / 10),
                totalRecords = total
            };

            return resDTO;
        }
    
        public async Task PatchUser(AdminPatchUserReqDTO reqDTO)
        {
            var repo = unitOfWork.GetRepository<User>();

            var user = await repo.Single(u => u.Id == reqDTO.Id);

            if(user == null)
            {
                throw new CustomHttpException("User không tồn tại", 404);
            }

            CommonHelper.Print(reqDTO);

            user.FirstName = reqDTO.FirstName;
            user.LastName = reqDTO.LastName;
            user.Email = reqDTO.Email;
            user.Role = reqDTO.Role;

            await repo.Update(user);
            await unitOfWork.SaveChanges();
        }
    
        public async Task DeleteUser(long id)
        {
            var repo = unitOfWork.GetRepository<User>();

            var user = await repo.Single(u => u.Id == id);

            if(user == null || user.IsDeleted == true)
            {
                throw new CustomHttpException("User không tồn tại hoặc đã bị xóa", 404);
            }

            user.IsDeleted = true;

            await repo.Update(user);
            await unitOfWork.SaveChanges();
        }
    
        public async Task PostCategory(AdminPostCategoryReqDTO reqDTO)
        {
            var cateEntity = mapper.Map<Category>(reqDTO);

            cateEntity.Image = reqDTO.FileName;

            var repo = unitOfWork.GetRepository<Category>();

            await repo.Update(cateEntity);
            await unitOfWork.SaveChanges();
        }
    
        public async Task<object> GetCategory(int page)
        {
            var repo = unitOfWork.GetRepository<Category>();

            var skipCount = (page - 1) * 10;

            var cates = (
                from x in await repo.QueryAll()
                select new {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Image,
                    x.IsDeleted
                }
            ).Skip(skipCount).Take(10).ToList();

            var total = await repo.Count();

            var resDTO = new
            {
                data = cates,
                page = page,
                totalPage = Math.Ceiling((double)total / 10),
                totalRecords = total
            };

            return resDTO;
        }

        public async Task<Category> DeleteCategory(long id)
        {
            var repo = unitOfWork.GetRepository<Category>();

            var cate = await repo.Single(u => u.Id == id);

            if (cate == null || cate.IsDeleted == true)
            {
                throw new CustomHttpException("Category không tồn tại hoặc đã bị xóa", 404);
            }

            cate.IsDeleted = true;

            await repo.Update(cate);
            await unitOfWork.SaveChanges();

            return cate;
        }

        public async Task RestoreCategory(long id) {
            var repo = unitOfWork.GetRepository<Category>();

            var cate = await repo.Single(u => u.Id == id);

            if (cate == null || cate.IsDeleted == false)
            {
                throw new CustomHttpException("Category không tồn tại hoặc đang hoạt động", 404);
            }

            cate.IsDeleted = false;

            await repo.Update(cate);
            await unitOfWork.SaveChanges();
        }
        public async Task PostUser(AdminPostUserReqDTO reqDTO)
        {
            var repo = unitOfWork.GetRepository<User>();

            var user = mapper.Map<User>(reqDTO);

            user.Password =  CommonHelper.ComputeHash("123123123");

            await repo.Add(user);
            await unitOfWork.SaveChanges();
        }
    
        public async Task<object> GetHighestOrders(int page) {
            var orderRepo = unitOfWork.GetRepository<Order>();
            var orderDetailRepo = unitOfWork.GetRepository<OrderDetail>();
            var productRepo = unitOfWork.GetRepository<Product>();
            var userRepo = unitOfWork.GetRepository<User>();

            var today = DateTime.Now.Date.AddDays(-1);

            var res = (
                from x in await orderRepo.QueryAll()
                join y in await orderDetailRepo.QueryAll() on x.Id equals y.OrderId
                join z in await productRepo.QueryAll() on y.ProductId equals z.Id
                join a in await userRepo.QueryAll() on x.ClerkId equals a.Id
                join b in await userRepo.QueryAll() on x.CustomerId equals b.Id
                where x.CreateAt.Date > today
                where x.ClerkId != null
                group new { x, y, z, a, b } by x.Id into g
                select new {
                    Id = g.Key,
                    TotalPrice = g.Sum(item => item.y.UnitPrice),
                    ProductName = g.First().z.Name,
                    ProductImage = g.First().z.Thumbnail,
                }
            ).ToList();
            
            return new {
                data = res
            };
        }

        public async Task<object> GetTodayOrders() {
            var orderRepo = unitOfWork.GetRepository<Order>();
            var orderDetailRepo = unitOfWork.GetRepository<OrderDetail>();
            var productRepo = unitOfWork.GetRepository<Product>();
            var userRepo = unitOfWork.GetRepository<User>();

            var today = DateTime.Now.Date.AddDays(-1);

            var res =(
                from x in await orderRepo.QueryAll()
                join y in await orderDetailRepo.QueryAll() on x.Id equals y.OrderId
                join z in await productRepo.QueryAll() on y.ProductId equals z.Id
                join a in await userRepo.QueryAll() on x.ClerkId equals a.Id
                join b in await userRepo.QueryAll() on x.CustomerId equals b.Id
                where x.CreateAt.Date > today
                where x.ClerkId != null
                select new {
                    x.Id,
                    CustomerEmail = b.Email,
                    ClerkEmail = a.Email,
                    x.CreateAt,
                    TotalPrice = y.UnitPrice,
                    ProductName = z.Name,
                    ProductImage = z.Thumbnail,
                }
            ).ToList();

            var total = await orderRepo.Count(x => x.CreateAt.Date > today);

            var resDTO = new
            {
                data = res,
            };

            return resDTO;
        } 

        public async Task<object> GetLowStockProducts(int page)
        {
            var productRepo = unitOfWork.GetRepository<Product>();
            var stockRepo = unitOfWork.GetRepository<Stock>();

            var skipCount = (page - 1) * 20;

            var res = (
                from x in await productRepo.QueryAll()
                join y in await stockRepo.QueryAll() on x.Id equals y.ProductId
                where y.Quantity < 10
                select new {
                    x.Id,
                    x.Name,
                    x.UnitPrice,
                    StockQuantity = y.Quantity
                }
            ).Skip(skipCount).Take(20).ToList();

            var total = await productRepo.Count();

            var resDTO = new
            {
                data = res,
                page = page,
                totalPage = Math.Ceiling((double)total / 20),
                totalRecords = total
            };

            return resDTO;
        }
    }
}
