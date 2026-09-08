using API.Authentication;
using Application.Commands.Authentication.Commands;
using Application.Commands.Org.Auth.Queries;
using Application.DTOs.OrgDb;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace API.Controllers.Org
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IJwtTokenService _tokenService;

        public AuthController(ISender sender, IJwtTokenService tokenService)
        {
            _sender = sender;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _sender.Send(command);

            if (result.StatusCode != HttpStatusCode.OK || result.Response is null)
                return Ok(new Result<LoginResponseDto>(result.StatusCode, null, result.Errors));

            var token = _tokenService.GenerateToken(result.Response);

            return Ok(new Result<LoginResponseDto>(result.StatusCode, new LoginResponseDto(result.Response, token), result.Errors));
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
