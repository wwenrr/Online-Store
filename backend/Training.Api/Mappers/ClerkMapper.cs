using AutoMapper;
using Training.Api.Models.Requests.Api;
using Training.Api.Models.Requests.Clerk;
using Training.BusinessLogic.Dtos.Clerk;
using Training.DataAccess.Entities;

namespace Training.Api.Mappers
{
    public class ClerkMapper : Profile
    {
        public ClerkMapper() {
            CreateMap<PostProductReqModel, PostProductReqDTO>();
            CreateMap<PostProductReqDTO, Product>();

            CreateMap<PostProductImageReqModel, PostProductImageReqDTO>();
            CreateMap<PostProductImageReqDTO, ProductImage>();

            CreateMap<PostStockEventReqModel, PostStockEventReqDTO>();
            CreateMap<PostStockEventReqDTO, StockEvent>();
        }
    }
}
