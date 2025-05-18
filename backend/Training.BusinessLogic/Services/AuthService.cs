using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Training.CustomException;
using Training.BusinessLogic.Dtos.Auth;
using Training.Common.Helpers;
using Training.DataAccess.Entities;
using Training.Repository.Repositories;
using Training.Repository.UoW;

namespace Training.BusinessLogic.Services
{
    public interface IAuthService
    {
        Task<string> RegisterService(RegisterReqDTO input);
        Task<string> LoginService(LoginReqDTO reqDTO);
    }
    public class AuthService(
        IMapper mapper,
        IUnitOfWork unitOfWork) : IAuthService
    {
        public async Task<string> RegisterService(RegisterReqDTO reqDTO)
        {
            var repo = unitOfWork.GetRepository<User>();

            if (await repo.Any(u => u.Email == reqDTO.Email))
            {
                throw new CustomHttpException("Email đã tồn tại", 409);
            }

            CommonHelper.EmailCheck(reqDTO.Email);
            CommonHelper.PasswdCheck(reqDTO.Password);

            var entity = mapper.Map<User>(reqDTO);
            entity.Password = entity.Password.ComputeHash();

            await repo.Add(entity);
            await unitOfWork.SaveChanges();

            return JwtHelper.GenToken(entity.Email, entity.Password.ComputeHash());
        }
    
        public async Task<string> LoginService(LoginReqDTO reqDTO)
        {
            //CommonHelper.Print(reqDTO.Passw);

            Console.WriteLine(reqDTO.Password.ComputeHash());
            var repo = unitOfWork.GetRepository<User>();

            if (!await repo.Any(u =>
                (u.Email == reqDTO.Email && u.Password == reqDTO.Password.ComputeHash())
            ))
            {
                throw new CustomHttpException("Tài khoản hoặc mật khẩu không khớp", 400);
            }


            return JwtHelper.GenToken(reqDTO.Email, reqDTO.Password.ComputeHash().ComputeHash());
        }    
    }
}
