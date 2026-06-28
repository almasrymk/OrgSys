using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.OrgDb
{
    public class CheckEmailAndPasswordDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
