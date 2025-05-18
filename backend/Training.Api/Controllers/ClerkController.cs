using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Training.Api.Models.Requests.Api;
using Training.Api.Models.Requests.Clerk;
using Training.Api.Models.Responses.Base;
using Training.BusinessLogic.Dtos.Clerk;
using Training.BusinessLogic.Services;
using Training.Common.Helpers;

namespace Training.Api.Controllers
{
    [ApiController]
    [Route("api/clerk")]
    public class ClerkController(
        ILogger<BaseController> logger,
        IMapper mapper,
        IClerkService clerkService
        ) : BaseController(logger, mapper)
    {
        [HttpPost("product")]
        public async Task<IActionResult> PostProduct([FromForm] PostProductReqModel reqModel)
        {
            var context = HttpContext;
            var fileName = await FileHelper.SaveImage(reqModel.ThumbnailFile);
            reqModel.Thumbnail = fileName;

            var response = new ResultRes<object>();
            var reqDTO = mapper.Map<PostProductReqDTO>(reqModel);
            reqDTO.Email = context.Items["email"] as String;

            try {
                await clerkService.PostProduct(reqDTO);
            } catch (Exception ex) {
                FileHelper.DeleteImage(fileName);
                throw ex;
            }

            var resModel = "Cập nhật sản phẩm thành công";

            response.Success = true;
            response.Result = resModel;

            return Ok(response);
        }
    
        [HttpPost("product/{productId}/image")]
        public async Task<IActionResult> PostProductImage(int productId, [FromForm] PostProductImageReqModel reqModel)
        {
            var fileName = await FileHelper.SaveImage(reqModel.ImageFile);  
            reqModel.Path = fileName;
            reqModel.ProductID = productId;

            var reqDTO = mapper.Map<PostProductImageReqDTO>(reqModel);

            try {
                await clerkService.PostProductImage(reqDTO);
            } catch (Exception ex) {
                FileHelper.DeleteImage(fileName);
                throw ex;
            }

            var response = new ResultRes<object>();
            response.Success = true;
            response.Result = "Cập nhật ảnh sản phẩm thành công";

            return Ok(response);
        }
    
        [HttpPost("stockEvent/{stockId}")]
        public async Task<IActionResult> PostStockEvent(int stockId, [FromBody] PostStockEventReqModel reqModel)
        {
            var reqDTO = mapper.Map<PostStockEventReqDTO>(reqModel);
            reqDTO.StockID = stockId;
            await clerkService.PostStockEvent(reqDTO);

            var response = new ResultRes<object>();
            response.Success = true;
            response.Result = "Cập nhật sự kiện kho thành công";

            return Ok(response);
        }
    
        [HttpGet("order")]
        public async Task<IActionResult> GetOrder([FromQuery] string orderType, [FromQuery] int page)
        {
            var context = HttpContext;
            var email = context.Items["email"] as String;

            var order = await clerkService.GetOrder(orderType, email, page);

            var response = new ResultRes<object>();
            response.Success = true;
            response.Result = order;

            return Ok(response);
        }
    
        [HttpPatch("order/approve/{orderId}")]
        public async Task<IActionResult> ReviewOrder(int orderId)
        {
            var context = HttpContext;
            var email = context.Items["email"] as String;

            await clerkService.ReviewOrder(orderId, email);

            var response = new ResultRes<object>();
            response.Success = true;
            response.Result = "Đã duyệt đơn hàng";

            return Ok(response);
        }
    
        [HttpDelete("product/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var context = HttpContext;
            var email = context.Items["email"] as String;

            await clerkService.DeleteProduct(productId, email);

            var response = new ResultRes<object>();
            response.Success = true;
            response.Result = "Đã xóa sản phẩm";

            return Ok(response);
        }

        [HttpDelete("product/image/{imageId}")]
        public async Task<IActionResult> DeleteProductImage(int imageId)
        {
            await clerkService.DeleteProductImage(imageId);
            
            var response = new ResultRes<object>();
            response.Success = true;
            response.Result = "Đã xóa ảnh sản phẩm";

            return Ok(response);
        }
    
        [HttpGet("product/{productId}/stockEvent/{page}")]
        public async Task<IActionResult> GetStockEventPage(int productId, int page)
        {
            var response = new ResultRes<object>();
            var resDTO = await clerkService.GetStockEventPage(productId, page);

            response.Success = true;
            response.Result = resDTO;

            return Ok(response);
        }
    
        [HttpGet("product/{page}")]
        public async Task<IActionResult> GetProduct(int page)
        {
            var resDTO = await clerkService.GetProduct(page);

            var response = new ResultRes<object>();

            response.Success = true;
            response.Result = resDTO;
            return Ok(response);
        }
    
        [HttpPatch("product/{productId}/restore")]
        public async Task<IActionResult> UnDeleteProduct(int productId)
        {
            await clerkService.UnDeleteProduct(productId);
            
            var response = new ResultRes<object>();
            response.Success = true;
            response.Result = "Đã khôi phục sản phẩm";

            return Ok(response);

        }
    
        [HttpPatch("product/{productId}/thumbnail/{imageId}")]
        public async Task<IActionResult> UpdateProductThumbnail(int productId, int imageId)
        {
            var response = new ResultRes<object>();

            await clerkService.UpdateProductThumbnail(productId, imageId);

            response.Success = true;
            response.Result = "Đã cập nhật ảnh sản phẩm";

            return Ok(response);
        }
    }
}
