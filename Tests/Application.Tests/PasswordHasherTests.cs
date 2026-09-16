using Administration.Application.Security;
using Xunit;

namespace Application.Tests;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void HashThenVerify_RoundtripSucceeds()
    {
        var hash = _hasher.HashPassword("P@ssw0rd");

        Assert.True(_hasher.VerifyHashedPassword(hash, "P@ssw0rd"));
    }

    [Fact]
    public void Verify_WrongPassword_Fails()
    {
        var hash = _hasher.HashPassword("P@ssw0rd");

        Assert.False(_hasher.VerifyHashedPassword(hash, "wrong-password"));
    }

    [Fact]
    public void TwoHashesOfTheSamePassword_Differ()
    {
        var first = _hasher.HashPassword("P@ssw0rd");
        var second = _hasher.HashPassword("P@ssw0rd");

        Assert.NotEqual(first, second);
        Assert.True(_hasher.VerifyHashedPassword(first, "P@ssw0rd"));
        Assert.True(_hasher.VerifyHashedPassword(second, "P@ssw0rd"));
    }

    [Fact]
    public void Verify_LegacyAesCiphertext_Fails()
    {
        // Pre-migration User.Password values were reversible AES ciphertext, not Identity hashes.
        const string leftoverAesCiphertext = "fTxWMjHA5MbUktJph2vqIlc9Gu1cU5MrbdYztkd5yec=";

        Assert.False(_hasher.VerifyHashedPassword(leftoverAesCiphertext, "P@ssw0rd"));
    }
}
