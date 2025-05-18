using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Training.Api.Models.Requests.Api;
using Training.Api.Models.Requests.Auth;
using Training.Api.Models.Responses.Base;
using Training.BusinessLogic.Dtos;
using Training.BusinessLogic.Dtos.Api;
using Training.BusinessLogic.Services;
using Training.CustomException;

namespace Training.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController(
        ILogger<BaseController> logger,
        IMapper mapper,
        IApiService apiService
        ) : BaseController(logger, mapper)
    {
        [HttpGet("info")]
        public async Task<IActionResult> Info()
        {
            var context = HttpContext;
            var email = context.Items["email"] as String;

            var resDTO = await apiService.GetUserInfo(email);
            var resModel = new
            {
                info=resDTO
            };

            var response = new ResultRes<object>();

            response.Success = true;
            response.Result = resModel;

            return Ok(response);
        }

        [HttpPatch("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswdReq req)
        {
            if(req.NewPassword != req.RepeatNewPassword)
            {
                throw new CustomHttpException("Mật khẩu không khớp!", 400);
            }

            if (req.NewPassword == req.OldPassword)
            {
                throw new CustomHttpException("Mật khẩu mới không được trùng mật khẩu cũ", 400);
            }

            var reqDTO = Mapper.Map<ChangePasswdReqDTO>(req);

            var context = HttpContext;
            var email = context.Items["email"] as String;

            var resDTO = await apiService.ChangePassword(reqDTO, email);
            var resModel = new
            {
                msg = "Đổi mật khẩu thành công",
                newToken = resDTO
            };

            var response = new ResultRes<object>();

            response.Result = resModel;
            response.Success = true;

            return Ok(response);
        }
    
        [HttpGet("product/{page}")]
        public async Task<IActionResult> GetProduct(int page)
        {
            var resDTO = await apiService.GetProduct(page);

            var response = new ResultRes<object>();

            response.Success = true;
            response.Result = resDTO;
            return Ok(response);
        }
    
        [HttpPost("order")]
        public async Task<IActionResult> PostOrder([FromBody] PostOrderReqModel reqModel)
        {
            var context = HttpContext;
            var email = context.Items["email"] as String;

            var response = new ResultRes<object>();

            var reqDTO = mapper.Map<PostOrderReqDTO>(reqModel);
            reqDTO.Email = email;

            await apiService.PostOrder(reqDTO);

            response.Success = true;
            response.Result = "Đặt hàng thành công";

            return Ok(response);    
        }
    
        [HttpGet("product/{searchType}/search/{page}")]
        public async Task<IActionResult> SearchProduct([FromQuery] string keyword, string searchType, int page)
        {
            // searchType = productName or categoryName

            var resDTO = await apiService.SearchProduct(keyword, searchType, page);
            
            var response = new ResultRes<object>();

            response.Success = true;
            response.Result = resDTO;

            return Ok(response);
        }
    
        [HttpGet("category")]
        public async Task<IActionResult> GetCategory()
        {
            var response = new ResultRes<object>();

            var resDTO = await apiService.GetCategory();

            var resModel = resDTO;

            response.Success = true;
            response.Result = resModel;

            return Ok(response);
        }
    
        [HttpGet("product/{productId}/detail")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var resDTO = await apiService.GetProductById(productId);

            var response = new ResultRes<object>();
            response.Success = true;
            response.Result = resDTO;

            return Ok(response);
        }
    
        [HttpPatch("info")]
        public async Task<IActionResult> PatchInfo([FromBody] PatchGetInfoReqModel reqModel)
        {
            var context = HttpContext;
            var email = context.Items["email"] as String;

            var reqDTO = mapper.Map<PatchGetInfoReqDTO>(reqModel);

            await apiService.PatchInfo(reqDTO, email);

            var response = new ResultRes<string>();

            response.Success = true;
            response.Result = "Cập nhật thông tin thành công";

            return Ok(response);
        }
    
        [HttpGet("order")]
        public async Task<IActionResult> GetOrder([FromQuery] int isReviewed)
        {
            var context = HttpContext;
            var email = context.Items["email"] as String;

            var resDTO = await apiService.GetOrder(isReviewed, email);

            var response = new ResultRes<object>();
            response.Success = true;
            response.Result = resDTO;

            return Ok(response);
        }
    
        [HttpGet("product/{page}/category/{categoryId}")]
        public async Task<IActionResult> GetProductByCategory(int page, int categoryId)
        {
            var resDTO = await apiService.GetProductByCategory(page, categoryId);
            
            var response = new ResultRes<object>();
            response.Success = true;
            response.Result = resDTO;

            return Ok(response);
        }
    }
}
