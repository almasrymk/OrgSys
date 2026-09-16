namespace Treasury.Domain.Tests;

public class FinancialTransactionTests
{
    [Fact]
    public void Direction_is_in_or_out()
    {
        Assert.Equal(1, (int)FinancialTransactionDirection.In);
        Assert.Equal(2, (int)FinancialTransactionDirection.Out);
    }

    [Fact]
    public void Transfer_is_a_reference_type_not_income_or_expense()
    {
        Assert.NotEqual(FinancialReferenceType.Income, FinancialReferenceType.Transfer);
        Assert.NotEqual(FinancialReferenceType.Expense, FinancialReferenceType.Transfer);
        Assert.Equal(11, (int)FinancialReferenceType.Transfer);
    }

    [Fact]
    public void Financial_account_is_either_cash_box_or_bank()
    {
        Assert.Equal(1, (int)FinancialAccountType.CashBox);
        Assert.Equal(2, (int)FinancialAccountType.Bank);
    }

    [Fact]
    public void Core_transaction_types_match_the_unified_model()
    {
        Assert.Equal(1, (long)FinancialTransactionType.OpeningBalance);
        Assert.Equal(2, (long)FinancialTransactionType.Receipt);
        Assert.Equal(3, (long)FinancialTransactionType.Payment);
        Assert.Equal(4, (long)FinancialTransactionType.TransferIn);
        Assert.Equal(11, (long)FinancialTransactionType.TransferOut);
    }

    [Fact]
    public void Transfer_header_defaults_exchange_rate_to_one()
    {
        var transfer = new FinancialTransfer
        {
            FromFinancialAccountId = 10,
            ToFinancialAccountId = 20,
            Amount = 100,
            CurrencyId = 1
        };

        Assert.Equal(1, transfer.ExchangeRate);
        Assert.NotEqual(transfer.FromFinancialAccountId, transfer.ToFinancialAccountId);
    }

    [Fact]
    public void Posted_transfer_is_two_legs_in_and_out()
    {
        var transfer = new FinancialTransfer { Id = 50, FromFinancialAccountId = 1, ToFinancialAccountId = 2, Amount = 250 };

        var outbound = new Financial
        {
            FinancialAccountId = transfer.FromFinancialAccountId,
            ContraFinancialAccountId = transfer.ToFinancialAccountId,
            FinancialTransferId = transfer.Id,
            Direction = FinancialTransactionDirection.Out,
            ReferenceType = FinancialReferenceType.Transfer,
            Amount = transfer.Amount
        };
        var inbound = new Financial
        {
            FinancialAccountId = transfer.ToFinancialAccountId,
            ContraFinancialAccountId = transfer.FromFinancialAccountId,
            FinancialTransferId = transfer.Id,
            Direction = FinancialTransactionDirection.In,
            ReferenceType = FinancialReferenceType.Transfer,
            Amount = transfer.Amount
        };

        Assert.Equal(transfer.Id, outbound.FinancialTransferId);
        Assert.Equal(transfer.Id, inbound.FinancialTransferId);
        Assert.Equal(FinancialTransactionDirection.Out, outbound.Direction);
        Assert.Equal(FinancialTransactionDirection.In, inbound.Direction);
        Assert.Equal(outbound.Amount, inbound.Amount);
    }

    [Fact]
    public void Financial_account_defaults_to_active()
    {
        var account = new FinancialAccount { Name = "Main cash", FinancialAccountType = FinancialAccountType.CashBox };

        Assert.True(account.IsActive);
    }
}
