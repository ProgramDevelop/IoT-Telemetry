using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Telemetry.Web.Services.Auth;
using Telemetry.Web.ViewModels.User;

namespace Telemetry.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;

        public TokenController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(LoginModel model)
        {
            if (_authService.CheckCredentials(model.Email, model.Password))
            {
                return new ObjectResult(GenerateToken(model.Email));
            }
            return BadRequest();
        }

        private string GenerateToken(string email)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisisasecreteforauth"));
            var signingCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtHeader = new JwtHeader(signingCredential);
            var jwtPayload = new JwtPayload(claims);
            var token = new JwtSecurityToken(jwtHeader, jwtPayload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}