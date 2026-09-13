using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 1- Data
        // 1-1- Organization: Organization.Application.MappingProfile (Branch, Shift, Table)
        // Stock, Product, ProductUnit, Property, Transaction, TransactionType, Inventory ->
        // Inventory.Application.MappingProfile

        // 1-2- Security: Administration.Application.MappingProfile (Role, User)

        // 1-3- Products: Classification, Unit -> MasterData.Application.MappingProfile

        // 1-4- Dealers, Invoices: Sales.Application.MappingProfile (DealerGroup, Dealer,
        // InvoiceType, Invoice)

        // 1-5- Financial: AccountType, Account, FiscalPeriod, FiscalYear -> Accounting.Application.MappingProfile
        // CashBox, BankBranch, Bank, FinancialAccount -> Treasury.Application.MappingProfile
        // Currency: MasterData.Application.MappingProfile

        // 1-6- Location: MasterData.Application.MappingProfile (Country, City, District)

        // 4-Financials: Journal -> Accounting.Application.MappingProfile; Financial -> Treasury.Application.MappingProfile

        // Preference: Administration.Application.MappingProfile

        // PaymentType, ReferenceType: MasterData.Application.MappingProfile
        // FinancialType: Treasury.Application.MappingProfile
    }
}
