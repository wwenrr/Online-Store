using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Training.Api.Models.Requests.Auth;
using Training.Api.Models.Responses.Base;
using Training.BusinessLogic.Dtos.Auth;
using Training.BusinessLogic.Services;
using Training.CustomException;

namespace Training.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(
        ILogger<BaseController> logger,
        IMapper mapper,
        IAuthService authService
        ) : BaseController(logger, mapper)
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReq req)
        {
            var response = new ResultRes<object>();

            var reqDTO = Mapper.Map<LoginReqDTO>(req);
            var token = await authService.LoginService(reqDTO);

            response.Success = true;
            response.Result = new
            {
                token=token
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterReq req)
        {
            if(req.Password != req.RepeatPassword)
            {
                throw new CustomHttpException("Mật khẩu không trùng nhau!", 400);
            }

            var response = new ResultRes<object>();
            response.Success = true;

            var reqDTO = Mapper.Map<RegisterReqDTO>(req);
            var token = await authService.RegisterService(reqDTO);

            response.Result = new 
            {
                token= token
            };

            return Ok(response);
        }
    }
}
