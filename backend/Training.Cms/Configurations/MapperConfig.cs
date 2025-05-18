using AutoMapper;
using Training.BusinessLogic.Dtos.Admin;
using Training.Cms.Models;
using Training.DataAccess.Entities;
using Training.DataAccess;
namespace Training.Cms.Configurations;

public class MapperConfig: Profile
{
    public MapperConfig()
    {
        CreateMap<User, AdminGetUserResDTO>();
        CreateMap<AdminPostUserReqDTO, User>();
        CreateMap<PostCategoryReqModel, AdminPostCategoryReqDTO>();
        CreateMap<AdminPostCategoryReqDTO, Training.DataAccess.Entities.Category>();
    }
} 