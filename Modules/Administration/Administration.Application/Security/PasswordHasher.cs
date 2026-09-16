using Microsoft.AspNetCore.Identity;

namespace Administration.Application.Security;

/// <summary>
/// Thin wrapper around <see cref="PasswordHasher{TUser}"/>. Identity's hasher is generic over
/// a user type but does not use the instance for PBKDF2 hashing, so a dummy <see cref="User"/>
/// is passed through.
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _inner = new();

    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        return _inner.HashPassword(null!, password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(providedPassword))
            return false;

        try
        {
            var result = _inner.VerifyHashedPassword(null!, hashedPassword, providedPassword);
            return result is PasswordVerificationResult.Success
                or PasswordVerificationResult.SuccessRehashNeeded;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
