using AutoMapper;
using Training.BusinessLogic.Dtos.Api;
using Training.Common.Helpers;
using Training.DataAccess.Entities;
using Training.Repository.UoW;
using Training.CustomException;
using MySqlConnector;
using Microsoft.EntityFrameworkCore;
using Training.BusinessLogic.Dtos;
using Training.Repository.Repositories;

namespace Training.BusinessLogic.Services
{
    public interface IApiService
    {
        Task<UserInfoResDTO> GetUserInfo(string email);
        Task<string> ChangePassword(ChangePasswdReqDTO reqDTO, string email);
        Task<object> GetProduct(int page);
        Task PostOrder(PostOrderReqDTO reqDTO);
        Task<object> SearchProduct(string keyword, string searchType, int page);
        Task<object> GetCategory();

        Task<object> GetProductById(int productId);
        Task<object> GetProductByCategory(int page, int categoryId);

        Task PatchInfo(PatchGetInfoReqDTO reqDTO, string email);
        Task<object> GetOrder(int isReviewed, string email);
    }
    public class ApiService(
        IMapper mapper,
        IUnitOfWork unitOfWork) : IApiService
    {
        public async Task<UserInfoResDTO> GetUserInfo(string email)
        {
            var repo = unitOfWork.GetRepository<User>();

            var user = await repo.Single(u => u.Email == email);

            var resDTO = mapper.Map<UserInfoResDTO>(user);

            return resDTO;
        }

        public async Task<string> ChangePassword(ChangePasswdReqDTO reqDTO, string email)
        {
            var repo = unitOfWork.GetRepository<User>();

            var user = await repo.Single(u => u.Email == email);

            if(!user.Password.Equals(CommonHelper.ComputeHash(reqDTO.OldPassword)))
            {
                throw new CustomHttpException("Nhập sai mật khẩu cũ", 400);
            }

            user.Password = CommonHelper.ComputeHash(reqDTO.NewPassword);

            await repo.Update(user);
            await unitOfWork.SaveChanges();

            var token = JwtHelper.GenToken(email, CommonHelper.ComputeHash(user.Password));

            return token;
        }
    
        public async Task<object> GetProduct(int page)
        {
            var productRepo = unitOfWork.GetRepository<Product>();
            var stockRepo = unitOfWork.GetRepository<Stock>();
            var userRepo = unitOfWork.GetRepository<User>();
            var categoryRepo = unitOfWork.GetRepository<Category>();

            var skipCount = (page-1) * 20;

            var res = (from x in await productRepo.QueryAll()
                join v in await categoryRepo.QueryAll() on x.CategoryId equals v.Id
                join w in await stockRepo.QueryAll() on x.Id equals w.ProductId
                where x.IsDeleted == false
                orderby x.CreatedAt descending
                select new {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Thumbnail,
                    x.UnitPrice,
                    CategoryName = v.Name,
                    StockQuantity = w.Quantity
                }
            ).Skip(skipCount).Take(20).ToList();

            var total = await productRepo.Count(p => p.IsDeleted == false);

            return new {
                data = res,
                page= page,
                totalPage= Math.Ceiling((double)total / 20),
                totalRecords = total
            };
        }
    
        public async Task PostOrder(PostOrderReqDTO reqDTO)
        {
            var orderRepo = unitOfWork.OrderRepository;
            var orderDetailRepo = unitOfWork.OrderDetailRepository;
            var productRepo = unitOfWork.GetRepository<Product>();
            var userRepo = unitOfWork.GetRepository<User>();

            if(reqDTO.Quantity <= 0)
            {
                throw new CustomHttpException("Số lượng phải lớn hơn 0", 400);
            }

            if(!(await productRepo.Any(p => p.Id == reqDTO.ProductId || p.IsDeleted == true)))
            {
                throw new CustomHttpException("Sản phẩm không tồn tại hoặc đã bị xóa", 404);
            }

            var customerId = (await userRepo.Single(u => u.Email == reqDTO.Email)).Id;

            var oderEntity = mapper.Map<Order>(reqDTO);
            oderEntity.CustomerId = customerId;
            oderEntity.CreateAt = DateTimeOffset.Now;

            await orderRepo.Add(oderEntity);
            await unitOfWork.SaveChanges();

            var orderDetailEntity = new OrderDetail
            {
                OrderId = oderEntity.Id,
                ProductId = reqDTO.ProductId,
                Quantity = reqDTO.Quantity,
                UnitPrice = (await productRepo.Single(p => p.Id == reqDTO.ProductId)).UnitPrice * reqDTO.Quantity
            };

            await orderDetailRepo.Add(orderDetailEntity);
            await unitOfWork.SaveChanges();

            CommonHelper.Print(oderEntity);
            CommonHelper.Print(orderDetailEntity);
        }
    
        public async Task<object> SearchProduct(string keyword, string searchType, int page)
        {
            var productRepo = unitOfWork.GetRepository<Product>();
            var stockRepo = unitOfWork.GetRepository<Stock>();
            var userRepo = unitOfWork.GetRepository<User>();
            var categoryRepo = unitOfWork.GetRepository<Category>();

            var skipCount = (page-1) * 40;

            if(searchType.Equals("productName")) {
                var res = (from x in await productRepo.QueryAll()
                    join y in await stockRepo.QueryAll() on x.Id equals y.ProductId
                    join z in await userRepo.QueryAll() on x.UpdateBy equals z.Id
                    join w in await userRepo.QueryAll() on x.CreateBy equals w.Id
                    join v in await categoryRepo.QueryAll() on x.CategoryId equals v.Id
                    where x.Name.Contains(keyword) && x.IsDeleted == false
                    select new {
                        x.Id,
                        x.Name,
                        x.Description,
                        x.Thumbnail,
                        x.UnitPrice,
                        x.CreatedAt,
                        x.UpdatedAt,
                        x.CategoryId,
                        StockId = y.Id,
                        StockQuantity = y.Quantity,
                        UserUpdateEmail = z.Email,
                        UserCreateEmail = w.Email,
                        CategoryName = v.Name
                    }
                ).Skip(skipCount).Take(40).ToList();

                return res;
            } else if(searchType.Equals("categoryName")) {
                var res = (from x in await productRepo.QueryAll()
                    join y in await stockRepo.QueryAll() on x.Id equals y.ProductId
                    join z in await userRepo.QueryAll() on x.UpdateBy equals z.Id
                    join w in await userRepo.QueryAll() on x.CreateBy equals w.Id
                    join v in await categoryRepo.QueryAll() on x.CategoryId equals v.Id
                    where v.Name.Contains(keyword) && x.IsDeleted == false  
                    select new {
                        x.Id,
                        x.Name,
                        x.Description,
                        x.Thumbnail,
                        x.UnitPrice,
                        x.CreatedAt,
                        x.UpdatedAt,
                        x.CategoryId,
                        StockId = y.Id,
                        StockQuantity = y.Quantity,
                        UserUpdateEmail = z.Email,
                        UserCreateEmail = w.Email,
                        CategoryName = v.Name
                    }
                ).Skip(skipCount).Take(40).ToList();

                return res;
            } else {
                throw new CustomHttpException("Loại tìm kiếm không hợp lệ, <productName> hoặc <categoryName>", 400);
            }
        }
    
        public async Task<object> GetCategory()
        {
            var repo = unitOfWork.GetRepository<Category>();

            var cates = (await repo.QueryCondition(u => u.IsDeleted == false)).ToList();

            var total = await repo.Count(u => u.IsDeleted == false);

            var resDTO = new
            {
                data = cates,
                totalRecords = total
            };

            return resDTO;
        }
    
        public async Task<object> GetProductByCategory(int page, int categoryId)
        {
            var productRepo = unitOfWork.GetRepository<Product>();
            var stockRepo = unitOfWork.GetRepository<Stock>();
            var userRepo = unitOfWork.GetRepository<User>();
            var categoryRepo = unitOfWork.GetRepository<Category>();

            var skipCount = (page-1) * 20;

            var res = (from x in await productRepo.QueryAll()
                join y in await stockRepo.QueryAll() on x.Id equals y.ProductId
                join z in await userRepo.QueryAll() on x.UpdateBy equals z.Id
                join w in await userRepo.QueryAll() on x.CreateBy equals w.Id
                join v in await categoryRepo.QueryAll() on x.CategoryId equals v.Id
                where x.CategoryId == categoryId
                orderby x.CreatedAt descending
                select new {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Thumbnail,
                    x.UnitPrice,
                    CategoryName = v.Name
                }
            ).Skip(skipCount).Take(20).ToList();

            var total = await productRepo.Count(u => u.CategoryId == categoryId && u.IsDeleted == false);
            

            var resDTO = new
            {
                data = res,
                totalRecords = total
            };

            return new {
                data = res,
                page= page,
                totalPage= Math.Ceiling((double)total / 20),
                totalRecords = total
            };
        }

        public async Task<object> GetProductById(int productId)
        {
            var productRepo = unitOfWork.GetRepository<Product>();
            var stockRepo = unitOfWork.GetRepository<Stock>();
            var userRepo = unitOfWork.GetRepository<User>();
            var categoryRepo = unitOfWork.GetRepository<Category>();
            var productImageRepo = unitOfWork.GetRepository<ProductImage>();

            var res = (from x in await productRepo.QueryAll()
                join y in await stockRepo.QueryAll() on x.Id equals y.ProductId
                join z in await userRepo.QueryAll() on x.UpdateBy equals z.Id
                join w in await userRepo.QueryAll() on x.CreateBy equals w.Id
                join v in await categoryRepo.QueryAll() on x.CategoryId equals v.Id
                where x.Id == productId
                select new {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Thumbnail,
                    x.UnitPrice,
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.CategoryId,
                    StockId = y.Id,
                    StockQuantity = y.Quantity,
                    UserUpdateEmail = z.Email,
                    UserCreateEmail = w.Email,
                    CategoryName = v.Name
                }
            ).FirstOrDefault();

            var images = (
                from x in await productImageRepo.QueryAll()
                where x.ProductID == productId
                orderby x.Order
                select new {
                    x.Path,
                    x.Id
                }
            ).ToList();

            return new {
                product = res,
                images = images
            };
        }
    
        public async Task PatchInfo(PatchGetInfoReqDTO reqDTO, string email)
        {
            var repo = unitOfWork.GetRepository<User>();

            var user = await repo.Single(u => u.Email == email);
            
            user.FirstName = reqDTO.FirstName;
            user.LastName = reqDTO.LastName;
            user.CivilianId = reqDTO.CivilianId;
            user.PhoneNumber = reqDTO.PhoneNumber;
            user.DateOfBirth = reqDTO.DateOfBirth;

            await repo.Update(user);
            await unitOfWork.SaveChanges();
        }
    
        public async Task<object> GetOrder(int isReviewed, string email)
        {
            var orderRepo = unitOfWork.GetRepository<Order>();
            var orderDetailRepo = unitOfWork.GetRepository<OrderDetail>();
            var productRepo = unitOfWork.GetRepository<Product>();
            var userRepo = unitOfWork.GetRepository<User>();

            var res = (
                from x in await orderRepo.QueryAll()
                join y in await orderDetailRepo.QueryAll() on x.Id equals y.OrderId
                join z in await productRepo.QueryAll() on y.ProductId equals z.Id
                join w in await userRepo.QueryAll() on x.CustomerId equals w.Id
                where w.Email == email
                where isReviewed == 0 ? x.ClerkId == null : x.ClerkId != null
                orderby x.CreateAt descending
                select new {
                    x.Id,
                    totalPrice = y.UnitPrice,
                    quantity = y.Quantity,
                    productName = z.Name,
                    thumbnail = z.Thumbnail,
                    orderDate = x.CreateAt,
                }
            );

            return res;
        }
    }
}
