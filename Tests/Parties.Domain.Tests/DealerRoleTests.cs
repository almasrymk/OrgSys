namespace Parties.Domain.Tests;

public class DealerRoleTests
{
    [Fact]
    public void Dealer_can_hold_customer_and_supplier_roles_on_the_same_identity()
    {
        var dealer = new Dealer
        {
            Id = 9,
            Name = "ABC Trading",
            TypeId = (long)DealerType.Supplier
        };

        dealer.CustomerProfile = new CustomerProfile { DealerId = dealer.Id, AccountId = 100, CreditLimit = 5000 };
        dealer.SupplierProfile = new SupplierProfile { DealerId = dealer.Id, AccountId = 200 };

        Assert.Equal((long)DealerType.Supplier, dealer.TypeId);
        Assert.Equal(dealer.Id, dealer.CustomerProfile.DealerId);
        Assert.Equal(dealer.Id, dealer.SupplierProfile.DealerId);
        Assert.NotEqual(dealer.CustomerProfile.AccountId, dealer.SupplierProfile.AccountId);
    }

    [Fact]
    public void Party_type_is_identity_not_a_commercial_role()
    {
        var dealer = new Dealer
        {
            Name = "Person supplier",
            PartyType = PartyType.Person,
            TypeId = (long)DealerType.Supplier
        };

        Assert.Equal(PartyType.Person, dealer.PartyType);
        Assert.Equal((long)DealerType.Supplier, dealer.TypeId);
        Assert.NotEqual(typeof(PartyType), typeof(DealerType));
    }

    [Fact]
    public void Customer_profile_defaults_allow_credit_and_not_on_hold()
    {
        var profile = new CustomerProfile { DealerId = 1 };

        Assert.True(profile.IsCreditAllowed);
        Assert.False(profile.IsOnHold);
    }

    [Fact]
    public void Supplier_profile_defaults_approved_and_not_on_hold()
    {
        var profile = new SupplierProfile { DealerId = 1 };

        Assert.True(profile.IsApproved);
        Assert.False(profile.IsOnHold);
    }

    [Fact]
    public void Primary_role_is_client_or_supplier()
    {
        Assert.Equal(1, (long)DealerType.Client);
        Assert.Equal(2, (long)DealerType.Supplier);
    }
}
