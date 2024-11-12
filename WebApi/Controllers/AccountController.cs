using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Dtos;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService service;

        public AccountController( IAccountService service)
        {
            this.service = service;
        }

        [HttpPost("CreateAccount")]
        public  async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto payload)
        {
            var serviceRespons = new ServiceResponse<dynamic>();

            serviceRespons = await service.CreateAccountAsync(payload);

            return StatusCode(serviceRespons.StatusCode, serviceRespons);
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] LoginDto payload)
        {
            var serviceResponse = new ServiceResponse<dynamic>();

            serviceResponse = await service.Login(payload);

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }
    }
}
