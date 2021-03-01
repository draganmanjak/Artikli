
using Artikli.Authentication.Models;
using AutoMapper;
using DataAccess.UserManagement.Interfaces;
using DataAccess.UserManagement.Models;
using DataAccess.UserManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artikli.Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, UserViewModel>().ReverseMap();

            });
            _mapper = config.CreateMapper();
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model is not valid" });
            }
            ServiceResponse<string> response = await _authRepository.Login(model.Email, model.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
   [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model is not valid" });
            }
            ServiceResponse<ApplicationUser> response = await _authRepository.Register(_mapper.Map <ApplicationUser>(model),model.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
    }
}
