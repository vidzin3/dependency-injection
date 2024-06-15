using Contract;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contract;

namespace DependencyInjection.Controllers
{
    [ApiController]
    [Route("/api/")]
    [Authorize(Roles = "Admin")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountInterface accountInterface;
        public AccountController(IAccountInterface accountInterface) => this.accountInterface = accountInterface;

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await accountInterface.Login(request);
            return Ok(new { 
                token = response 
            });
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var response = await accountInterface.Register(request);
            return Ok(new
            {
                token = response
            });
        }
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestDto request)
        {
            var response = await accountInterface.RegisterAdmin(request);
            return Ok(new
            {
                token = response
            });
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshTokenController(TokenModel token)
        {
            var response = await accountInterface.RefreshToken(token);
            return Ok(response);
        }
    }
}
