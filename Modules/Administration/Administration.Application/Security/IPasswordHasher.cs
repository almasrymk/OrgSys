namespace Administration.Application.Security;

/// <summary>
/// Hashes and verifies user passwords. Implementation wraps ASP.NET Identity's
/// <c>PasswordHasher&lt;T&gt;</c> (per-hash salt, versioned iteration counts).
/// </summary>
public interface IPasswordHasher
{
    string HashPassword(string password);

    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}
