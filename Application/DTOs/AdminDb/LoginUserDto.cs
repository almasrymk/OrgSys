using Domain.Entities;

namespace Application.DTOs
{
    public class LoginUserDto : LoginUser
    {
        public string? ClientName { get; set; }

        public string? Schema { get; set; }

        public string? NewPassword { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}