namespace Accounting.Domain.Tests;

public class AccountTests
{
    private static Account NewAccount(bool isPostable = true) => new()
    { Id = 1, Name = "Cash", AccountTypeId = 1, IsPostable = isPostable };

    [Fact]
    public void EnsurePostable_PostableActiveAccount_DoesNotThrow()
    {
        var account = NewAccount();

        var ex = Record.Exception(account.EnsurePostable);

        Assert.Null(ex);
    }

    [Fact]
    public void EnsurePostable_GroupParentAccount_ThrowsAccountNotPostable()
    {
        var account = NewAccount(isPostable: false);

        Assert.Throws<AccountNotPostableException>(account.EnsurePostable);
    }

    [Fact]
    public void EnsurePostable_DeactivatedAccount_ThrowsAccountNotPostable()
    {
        var account = NewAccount();
        account.Deactivate();

        Assert.Throws<AccountNotPostableException>(account.EnsurePostable);
    }

    [Fact]
    public void EnsurePostable_DeletedAccount_ThrowsAccountNotPostable()
    {
        var account = NewAccount();
        account.Status = Status.Deleted;

        Assert.Throws<AccountNotPostableException>(account.EnsurePostable);
    }

    [Fact]
    public void Activate_ReversesDeactivate_AccountBecomesPostableAgain()
    {
        var account = NewAccount();
        account.Deactivate();

        account.Activate();

        var ex = Record.Exception(account.EnsurePostable);
        Assert.Null(ex);
    }

    [Fact]
    public void MarkNonPostable_ThenMarkPostable_TogglesPostingEligibility()
    {
        var account = NewAccount();

        account.MarkNonPostable();
        Assert.Throws<AccountNotPostableException>(account.EnsurePostable);

        account.MarkPostable();
        var ex = Record.Exception(account.EnsurePostable);
        Assert.Null(ex);
    }

    [Fact]
    public void Rename_UpdatesName()
    {
        var account = NewAccount();

        account.Rename("Petty Cash");

        Assert.Equal("Petty Cash", account.Name);
    }

    [Fact]
    public void ChangeAccountType_UpdatesAccountTypeId()
    {
        var account = NewAccount();

        account.ChangeAccountType(7);

        Assert.Equal(7, account.AccountTypeId);
    }
}
