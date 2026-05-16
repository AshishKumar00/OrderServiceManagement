using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OrderServiceManagement.Application.DTOs;
using OrderServiceManagement.Application.Interfaces;
using OrderServiceManagement.Infrastructure.Data;

namespace OrderServiceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public string Login([FromBody] LoginRequestDto obj)
        {
            var token = _authService.Login(obj);
            return token;
        }
    }
}
