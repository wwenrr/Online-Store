using AutoMapper;
using Training.Api.Models.Requests.Api;
using Training.BusinessLogic.Dtos;
using Training.BusinessLogic.Dtos.Api;
using Training.DataAccess.Entities;

namespace Training.Api.Mappers
{
    public class ApiMapper : Profile
    {
        public ApiMapper()
        {
            CreateMap<User, UserInfoResDTO>();
            CreateMap<ChangePasswdReq, ChangePasswdReqDTO>();
            CreateMap<Product, GetProductResDTO>();
            CreateMap<PostOrderReqModel, PostOrderReqDTO>();
            CreateMap<PostOrderReqDTO, Order>();
            CreateMap<PatchGetInfoReqModel, PatchGetInfoReqDTO>();
            CreateMap<PatchGetInfoReqDTO, User>();
        }
    }
}
