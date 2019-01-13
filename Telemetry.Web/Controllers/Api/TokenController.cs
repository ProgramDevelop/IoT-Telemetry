using Microsoft.AspNetCore.Mvc;
using Telemetry.Web.Services.Auth;
using Telemetry.Web.Services.Jwt;
using Telemetry.Web.ViewModels.User;

namespace Telemetry.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtFactory _jwtFactory;

        public TokenController(IAuthService authService, IJwtFactory jwtFactory)
        {
            _authService = authService;
            _jwtFactory = jwtFactory;
        }

        [HttpPost]
        [Route("create")]
        public ActionResult<string> Create(LoginModel model)
        {
            if (_authService.CheckCredentials(model.Email, model.Password))
            {
                var token = _jwtFactory.GenerateToken(model.Email);
                return token;
            }
            return BadRequest();
        }
    }
}