using Application.Commands.Authentication.Commands;
using Application.Commands.Org.Auth.Queries;
using Application.DTOs.OrgDb;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace API.Controllers.Org
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _sender.Send(command);

            return Ok(result);
        }

        [HttpGet("CheckEmail")]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            return Ok(await _sender.Send(new CheckEmailQuery(email)));
        }

        [HttpPost("CheckPassword")]
        public async Task<IActionResult> CheckPassword([FromBody] CheckEmailAndPasswordDto checkEmailAndPasswordDto)
        {
            return Ok(await _sender.Send(new CheckPasswordQuery(checkEmailAndPasswordDto.Email, checkEmailAndPasswordDto.Password)));
        }
        [HttpGet("HavePassword")]
        public async Task<IActionResult> HavePassword(string email)
        {
            return Ok(await _sender.Send(new HavePasswordQuery(email)));
        }
    }
}
