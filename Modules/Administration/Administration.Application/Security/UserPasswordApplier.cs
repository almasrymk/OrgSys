namespace Administration.Application.Security;

/// <summary>
/// Applies Identity password hashing to a user DTO before it is mapped onto the entity.
/// Plaintext lives on <see cref="UserDto.NewPassword"/> (preferred) or <see cref="UserDto.Password"/>.
/// </summary>
internal static class UserPasswordApplier
{
    public static void Apply(UserDto dto, IPasswordHasher hasher, string? existingHash = null)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(hasher);

        var plaintext = !string.IsNullOrWhiteSpace(dto.NewPassword)
            ? dto.NewPassword
            : dto.Password;

        if (!string.IsNullOrWhiteSpace(plaintext) && plaintext != existingHash)
        {
            dto.Password = hasher.HashPassword(plaintext);
            dto.MustResetPassword = false;
        }
        else
        {
            dto.Password = existingHash;
        }

        dto.NewPassword = null;
        dto.ConfirmPassword = null;
    }
}
