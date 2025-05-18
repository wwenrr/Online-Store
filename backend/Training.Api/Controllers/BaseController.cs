﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training.Api.Models.Responses.Base;

namespace Training.Api.Controllers
{
    //[Authorize]
    [ApiController]
    public class BaseController(
        ILogger<BaseController> logger,
        IMapper mapper
        ) : ControllerBase
    {
        protected IMapper Mapper { get; } = mapper;
        protected ILogger<BaseController> Logger { get; } = logger;

        [NonAction]
        protected IActionResult InternalServerError(object? data)
        {
            if (data is ExecutionRes apiResult)
            {
                apiResult.Error = string.IsNullOrEmpty(apiResult.Error)
                    ? "Something went wrong. Please try again later"
                    : apiResult.Error;
            }

            return StatusCode(StatusCodes.Status500InternalServerError, data);
        }
    }
}
