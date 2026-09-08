using Application.DTOs;

namespace API.Authentication
{
    public record LoginResponseDto(UserDto User, string Token);
}
