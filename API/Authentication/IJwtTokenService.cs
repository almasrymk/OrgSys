
namespace API.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserDto user);
    }
}
