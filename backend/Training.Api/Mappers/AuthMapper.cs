using AutoMapper;
using Training.Api.Models.Requests.Auth;
using Training.BusinessLogic.Dtos.Auth;
using Training.DataAccess.Entities;

namespace Training.Api.Mappers
{
    public class AuthMapper : Profile
    {
        public AuthMapper()
        {
            CreateMap<RegisterReq, RegisterReqDTO>();
            CreateMap<RegisterReqDTO, User>();
            CreateMap<LoginReq, LoginReqDTO>();
        }
    }
}
