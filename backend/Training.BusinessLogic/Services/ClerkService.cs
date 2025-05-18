using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Training.BusinessLogic.Dtos.Clerk;
using Training.Common.Helpers;
using Training.DataAccess.Entities;
using Training.Repository.UoW;
using Training.CustomException;
using MySqlConnector;
using Microsoft.EntityFrameworkCore;

namespace Training.BusinessLogic.Services
{
    public interface IClerkService
    {
        Task PostProduct(PostProductReqDTO reqDTO);
        Task PostProductImage(PostProductImageReqDTO reqDTO);

        Task PostStockEvent(PostStockEventReqDTO reqDTO);
        Task<object> GetOrder(string orderType, string email, int page);
        Task ReviewOrder(int orderId, string email);
        Task DeleteProduct(int productId, string email);
        Task DeleteProductImage(int imageId);
        Task<object> GetStockEventPage(int productId, int page);

        Task<object> GetProduct(int page);
        Task UnDeleteProduct(int productId);
        Task UpdateProductThumbnail(int productId, int imageId);
    }
    public class ClerkService(
        IMapper mapper,
        IUnitOfWork unitOfWork) : IClerkService
    {
        public async Task PostProduct(PostProductReqDTO reqDTO)
        {
            var email = reqDTO.Email;

            var userRepo = unitOfWork.GetRepository<User>();
            var productRepo = unitOfWork.GetRepository<Product>();
            var categoryRepo = unitOfWork.GetRepository<Category>();

            var user = await userRepo.Single(u => u.Email == email);

            if(!(await categoryRepo.Any(u => u.Id == reqDTO.CategoryId && u.IsDeleted != true)))
            {
                throw new CustomHttpException("Category không tồn tại hoặc đã bị xóa", 404);
            }

            var entity = mapper.Map<Product>(reqDTO);

            entity.CreateBy = user.Id;
            entity.UpdateBy = user.Id;
            entity.CategoryId = reqDTO.CategoryId;

            var stockRepo = unitOfWork.StockRepository;

            await productRepo.Add(entity);
            await unitOfWork.SaveChanges();

            CommonHelper.Print(entity);

            var stock = new Stock();
            stock.ProductId = entity.Id;
            stock.Quantity = 0;

            await stockRepo.Add(stock);
            await unitOfWork.SaveChanges();
        }
    
        public async Task PostProductImage(PostProductImageReqDTO reqDTO)
        {
            var productImageRepo = unitOfWork.GetRepository<ProductImage>();
            var productRepo = unitOfWork.GetRepository<Product>();  

            if(!(await productRepo.Any(p => p.Id == reqDTO.ProductID)))
            {
                throw new CustomHttpException("Sản phẩm không tồn tại hoặc đã bị xóa", 404);
            }

            var ImageCount = await productImageRepo.Count(p => p.ProductID == reqDTO.ProductID);

            var entity = mapper.Map<ProductImage>(reqDTO);

            entity.Order = ImageCount;
            
            await productImageRepo.Add(entity);
            await unitOfWork.SaveChanges();
        }   
    
        public async Task PostStockEvent(PostStockEventReqDTO reqDTO)
        {
            CommonHelper.Print(reqDTO);

            var productRepo = unitOfWork.GetRepository<Product>();
            var stockRepo = unitOfWork.GetRepository<Stock>();
            var stockEventRepo = unitOfWork.GetRepository<StockEvent>();

            if(!(await stockRepo.Any(s => s.Id == reqDTO.StockID)))
            {
                throw new CustomHttpException("Kho không tồn tại hoặc đã bị xóa", 404);
            }

            var quantity = reqDTO.Quantity;
            var stockType = reqDTO.StockType;
            var stockEntity = await stockRepo.Single(s => s.Id == reqDTO.StockID);

            if(stockType == StockType.In)
            {
                stockEntity.Quantity += quantity;
            }
            else
            {
                stockEntity.Quantity -= quantity;

                if(stockEntity.Quantity < 0)
                {
                    throw new CustomHttpException("Số lượng kho không đủ", 400);
                }
            }

            var stockEventEntity = mapper.Map<StockEvent>(reqDTO);
            stockEventEntity.CreatedAt = DateTimeOffset.Now;
            stockEventEntity.Type = stockType == StockType.In ? Training.DataAccess.Entities.Type.In : Training.DataAccess.Entities.Type.Out;

            await stockRepo.Update(stockEntity);
            await stockEventRepo.Add(stockEventEntity);
            await unitOfWork.SaveChanges();
        }
    
        public async Task<object> GetOrder(string orderType, string email, int page)
        {
            var orderRepo = unitOfWork.GetRepository<Order>();
            var orderDetailRepo = unitOfWork.GetRepository<OrderDetail>();
            var userRepo = unitOfWork.GetRepository<User>();
            var productRepo = unitOfWork.GetRepository<Product>();
            var user = await userRepo.Single(u => u.Email == email);
            var categoryRepo = unitOfWork.GetRepository<Category>();
            var stockRepo = unitOfWork.StockRepository;

            if (orderType.Equals("reviewed")) {
                var res = (
                    from x in await orderRepo.QueryAll()
                    join y in await orderDetailRepo.QueryAll() on x.Id equals y.OrderId
                    join z in await productRepo.QueryAll() on y.ProductId equals z.Id
                    join v in await categoryRepo.QueryAll() on z.CategoryId equals v.Id
                    join w in await userRepo.QueryAll() on x.CustomerId equals w.Id
                    join a in await userRepo.QueryAll() on x.ClerkId equals a.Id
                    where x.ClerkId != null
                    orderby x.CreateAt descending
                    select new {
                        x.Id,
                        x.CreateAt,
                        y.Quantity,
                        z.Name,
                        z.Thumbnail,
                        ProductUnitPrice = z.UnitPrice,
                        TotalPrice = y.UnitPrice,
                        CategoryId = z.CategoryId,
                        CategoryName = v.Name,
                        CustomerEmail = w.Email,
                        ClerkEmail = a.Email
                    }
                ).Skip((page - 1) * 10).Take(10).ToList();

                return res;
            } else if (orderType.Equals("pending")) {
                var res = (
                    from x in await orderRepo.QueryAll()
                    join y in await orderDetailRepo.QueryAll() on x.Id equals y.OrderId
                    join z in await productRepo.QueryAll() on y.ProductId equals z.Id
                    join v in await categoryRepo.QueryAll() on z.CategoryId equals v.Id
                    join w in await userRepo.QueryAll() on x.CustomerId equals w.Id
                    join s in await stockRepo.QueryAll() on y.ProductId equals s.ProductId
                    where x.ClerkId == null
                    orderby x.CreateAt descending
                    select new {
                        x.Id,
                        x.CreateAt,
                        z.Name,
                        z.Thumbnail,
                        BuyQuantity = y.Quantity,
                        ProductUnitPrice = z.UnitPrice,
                        TotalPrice = y.UnitPrice,
                        CategoryName = v.Name,
                        CustomerEmail = w.Email,
                        StockQuantity = s.Quantity,
                        ProductId = y.ProductId,
                        StockId = s.Id
                    }
                ).Skip((page - 1) * 10).Take(10).ToList();

                return res;
            } else {
                throw new CustomHttpException("Loại đơn hàng không hợp lệ, <reviewed> hoặc <pending>", 400);
            }
        }
    
        public async Task ReviewOrder(int orderId, string email)
        {
            var orderRepo = unitOfWork.GetRepository<Order>();
            var orderDetailRepo = unitOfWork.GetRepository<OrderDetail>();
            var productRepo = unitOfWork.GetRepository<Product>();
            var stockRepo = unitOfWork.GetRepository<Stock>();
            var userRepo = unitOfWork.GetRepository<User>();
            var stockEventRepo = unitOfWork.GetRepository<StockEvent>();

            var res = (
                from x in await orderRepo.QueryAll()
                join y in await orderDetailRepo.QueryAll() on x.Id equals y.OrderId
                join z in await productRepo.QueryAll() on y.ProductId equals z.Id
                join s in await stockRepo.QueryAll() on y.ProductId equals s.ProductId
                where x.Id == orderId
                select new {
                    x.Id,
                    x.ClerkId,
                    StockId = s.Id,
                    totalPrice = y.UnitPrice,
                    orderQuantity = y.Quantity,
                    stockQuantity = s.Quantity
                }
            ).ToList();            

            if(res.Count == 0)
            {
                throw new CustomHttpException("Đơn hàng không tồn tại", 404);
            }

            var order = res[0];
            
            if(order.orderQuantity > order.stockQuantity)
            {
                throw new CustomHttpException("Số lượng sản phẩm trong kho không đủ", 400);
            }

            if(order.ClerkId != null)
            {
                throw new CustomHttpException("Đơn hàng đã được duyệt bởi người khác", 400);
            }

            CommonHelper.Print(order);

            var orderEntity = await orderRepo.Single(o => o.Id == order.Id);
            orderEntity.ClerkId = (await userRepo.Single(u => u.Email == email)).Id;

            var stockEntity = await stockRepo.Single(s => s.Id == order.StockId);
            stockEntity.Quantity -= order.orderQuantity;
            orderEntity.CreateAt = DateTimeOffset.Now;

            var stockEventEntity = new StockEvent();
            stockEventEntity.StockID = order.StockId;
            stockEventEntity.Reason = $"Đơn hàng đã được duyệt bởi {email}";
            stockEventEntity.Type = Training.DataAccess.Entities.Type.Out;
            stockEventEntity.Quantity = order.orderQuantity;
            stockEventEntity.CreatedAt = DateTimeOffset.Now;

            await stockRepo.Update(stockEntity);
            await stockEventRepo.Add(stockEventEntity);
            await orderRepo.Update(orderEntity);
            await unitOfWork.SaveChanges();

            CommonHelper.Print(orderEntity);
        }
    
        public async Task DeleteProduct(int productId, string email)
        {
            var productRepo = unitOfWork.GetRepository<Product>();
            var productImageRepo = unitOfWork.GetRepository<ProductImage>();
            var userRepo = unitOfWork.GetRepository<User>();

            var product = await productRepo.Single(p => p.Id == productId);

            if(product == null)
            {
                throw new CustomHttpException("Sản phẩm không tồn tại", 404);
            }
            
            if(product.IsDeleted == true    )
            {
                throw new CustomHttpException("Sản phẩm đã bị xóa", 404);
            }

            product.IsDeleted = true;
            product.UpdateBy = (await userRepo.Single(u => u.Email == email)).Id;
            product.UpdatedAt = DateTimeOffset.Now;
            await productRepo.Update(product);
            await unitOfWork.SaveChanges();
        }
    
        public async Task DeleteProductImage(int imageId)
        {
            var productImageRepo = unitOfWork.GetRepository<ProductImage>();
            var productImage = await productImageRepo.Single(p => p.Id == imageId);

            if(productImage == null)
            {
                throw new CustomHttpException("Ảnh sản phẩm không tồn tại", 404);
            }
            
            await productImageRepo.Delete(productImage);
            await unitOfWork.SaveChanges();
        }
    
        public async Task<object> GetStockEventPage(int productId, int page)
        {
            var stockEventRepo = unitOfWork.GetRepository<StockEvent>();
            var stockRepo = unitOfWork.GetRepository<Stock>();
            var productRepo = unitOfWork.GetRepository<Product>();
        
            var res = (
                from x in await productRepo.QueryAll()
                join y in await stockRepo.QueryAll() on x.Id equals y.ProductId
                join z in await stockEventRepo.QueryAll() on y.Id equals z.StockID
                where x.Id == productId
                orderby z.CreatedAt descending
                select new {
                    Type = z.Type == Training.DataAccess.Entities.Type.In ? "Nhập" : "Xuất",
                    z.Quantity,
                    z.Reason,
                    z.CreatedAt
                }
            ).Skip((page - 1) * 10).Take(10).ToList();

            var stock = await stockRepo.Single(s => s.ProductId == productId);

            return new {
                Stock = new {
                    Id = stock.Id,
                    Quantity = stock.Quantity
                },
                StockEvents = res
            };
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
                orderby x.CreatedAt descending
                select new {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Thumbnail,
                    x.UnitPrice,
                    CategoryName = v.Name,
                    x.IsDeleted,
                    x.UpdatedAt
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
    
        public async Task UnDeleteProduct(int productId)
        {
            var repo = unitOfWork.GetRepository<Product>();

            var product = await repo.Single(p => p.Id == productId);
            
            if(product == null)
            {
                throw new CustomHttpException("Sản phẩm không tồn tại", 404);
            }

            product.IsDeleted = false;

            await repo.Update(product);
            await unitOfWork.SaveChanges();
        }
    
        public async Task UpdateProductThumbnail(int productId, int imageId) 
        {
            var productRepo = unitOfWork.GetRepository<Product>();
            var productImageRepo = unitOfWork.GetRepository<ProductImage>();

            var eProduct = await productRepo.Single(p => p.Id == productId);
            var eEimage = await productImageRepo.Single(i => i.Id == imageId);

            var temp = eProduct.Thumbnail;
            eProduct.Thumbnail = eEimage.Path;
            eEimage.Path = temp;

            await productRepo.Update(eProduct);
            await productImageRepo.Update(eEimage);
            await unitOfWork.SaveChanges();
        }
    }
}
