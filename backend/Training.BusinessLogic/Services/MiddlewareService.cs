using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Training.BusinessLogic.Dtos.Middleware;
using Training.Common.Helpers;
using Training.CustomException;
using Training.DataAccess.Entities;
using Training.Repository.UoW;

namespace Training.BusinessLogic.Services
{
    public interface IMiddlewareService
    {
        Task<ApiMiddlewareResDTO> HandleApiMiddleware(HttpContext context);
    }
    public class MiddlewareService(IUnitOfWork unitOfWork) : IMiddlewareService
    {
        public async Task<ApiMiddlewareResDTO> HandleApiMiddleware(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();

            if (authHeader == "")
            {
                throw new CustomHttpException("Không tìm thấy token!", 403);
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            JwtHelper.ValidateToken(token);

            var decoded = JwtHelper.DecodeToken(token);

            var emailClaim = decoded.Claims.FirstOrDefault(c => c.Type == "email").Value;
            var keyClaim = decoded.Claims.FirstOrDefault(c => c.Type == "key").Value;

            var repo = unitOfWork.GetRepository<User>();

            var user = await repo.Single(u => u.Email == emailClaim);

            if ((CommonHelper.ComputeHash(user.Password)) != keyClaim)
            {
                throw new CustomHttpException("Token không còn hiệu lực!", 403);
            }

            return new ApiMiddlewareResDTO ( user.Email, user.Role);
        }
    }
}
