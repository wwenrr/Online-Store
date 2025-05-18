using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Training.Api.Models.Requests.Admin;
using Training.Api.Models.Responses.Base;
using Training.BusinessLogic.Dtos.Admin;
using Training.BusinessLogic.Services;
using Training.Common.Helpers;

namespace Training.Api.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController(
        ILogger<BaseController> logger,
        IMapper mapper,
        IAdminService adminService) : BaseController(logger, mapper)
    {
        
    }
}
