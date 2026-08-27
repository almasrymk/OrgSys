using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 1- Data
        // 1-1- Organization
        BranchMappingProfile();
        StockMappingProfile();

        // 1-2- Security
        RoleMappingProfile();
        UserMappingProfile();

        // 1-3- Products
        ProductUnitMappingProfile();
        ProductMappingProfile();
        ClassificationMappingProfile();
        UnitMappingProfile();

        // 1-4- Dealers
        DealerGroupMappingProfile();
        DealerMappingProfile();

        // 1-5- Financial
        AccountTypeMappingProfile();
        AccountMappingProfile();
        CashBoxMappingProfile();
        BankBranchMappingProfile();
        BankMappingProfile();
        FinancialAccountMappingProfile();
        CurrencyMappingProfile();
        FiscalPeriodMappingProfile();
        FiscalYearMappingProfile();

        // 1-6- Location
        CountryMappingProfile();
        CityMappingProfile();
        DistrictMappingProfile();

        // 2-Invoices
        InvoiceTypeMappingProfile();
        InvoiceMappingProfile();

        // 3-Transactions
        TransactionMappingProfile();
        InventoryMappingProfile();

        // 4-Financials
        JournalMappingProfile();
        FinancialMappingProfile();

        PreferenceMappingProfile();

        PaymentTypeMappingProfile();      
        TransactionTypeMappingProfile();
        FinancialTypeMappingProfile();
    }
}