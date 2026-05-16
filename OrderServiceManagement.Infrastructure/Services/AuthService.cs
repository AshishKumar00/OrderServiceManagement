using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using OrderServiceManagement.Application.DTOs;
using OrderServiceManagement.Application.Interfaces;
using OrderServiceManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Claims;
using System.Text;
using System.Text;
using System.Threading.Tasks;


namespace OrderServiceManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _context;
        private readonly IConfiguration _configuration; 
        public AuthService(AuthDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string Login(LoginRequestDto loginRequest)
        {
            if (loginRequest.Username != null && loginRequest.Password != null)
            {
                var user = _context.Users.SingleOrDefault(s => s.Username == loginRequest.Username && s.PasswordHash == loginRequest.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("Username", user.Username),
                        new Claim(ClaimTypes.Role, user.Role)

                    };

                    //  Create security key and signing credentials
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    //  Generate and serialize the token string
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: credentials);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwtToken;

                }
                else
                {
                    throw new Exception(" username Invalid");
                }
            }
            else
            {
                throw new Exception("Credentials are not valid");
            }
        }
    }
}
    