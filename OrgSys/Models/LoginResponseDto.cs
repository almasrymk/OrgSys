using Application.DTOs;

namespace OrgSys.Models
{
    // Mirrors API.Authentication.LoginResponseDto - the API's login endpoint now wraps the
    // authenticated user together with the JWT it must present on every subsequent call.
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
