using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.Api.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var token = await _tokenService.GetToken(user);

            if (string.IsNullOrWhiteSpace(token))
                return Unauthorized("Invalid username or password.");

            return Ok(token);
        }

    }
}
