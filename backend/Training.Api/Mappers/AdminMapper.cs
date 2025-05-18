using AutoMapper;
using Training.Api.Models.Requests.Admin;
using Training.BusinessLogic.Dtos.Admin;
using Training.DataAccess.Entities;

namespace Training.Api.Mappers
{
    public class AdminMapper : Profile
    {
        public AdminMapper()
        {
            CreateMap<User, AdminGetUserResDTO>();
            CreateMap<AdminPostUserReqModel, AdminPostUserReqDTO>();
            CreateMap<AdminPostUserReqDTO, User>();
            CreateMap<AdminPatchUserReqModel, AdminPatchUserReqDTO>();
            CreateMap<AdminPostCategoryReqModel, AdminPostCategoryReqDTO>();
            CreateMap<AdminPostCategoryReqDTO, Category>();
        }
    }
}
