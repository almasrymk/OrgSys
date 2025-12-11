using AutoMapper;
using AutoMapper.Configuration;
using Entity.Model;
using Entity.ModelView;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Entity
{
    public class MapperConfig : Profile
    {
        public IMapper Mapper { get; set; }
        public MapperConfig()
        {
            //var cfg = new MapperConfigurationExpression();
            
            AdminConfig(this);
            OrgConfig(this);
            //ILoggerFactory loggerFactory = (ILoggerFactory)Activator.CreateInstance(typeof(ILoggerFactory));
            //cfg.AddProfile(this);
            //Mapper = new MapperConfiguration(cfg, loggerFactory).CreateMapper();
        }

        public void AdminConfig(Profile cfg)
        {
            // Client
            cfg.CreateMap<ClientModelView, Client>();
            cfg.CreateMap<Client, ClientModelView>()
            .ForMember(d => d.TypeActivityName, o => o.MapFrom(s => s.TypeActivity.Name))
            .ForMember(d => d.NationalityName, o => o.MapFrom(s => s.Nationality.Name));
            //

            // Client Plan
            cfg.CreateMap<ClientPlanModelView, ClientPlan>();
            cfg.CreateMap<ClientPlan, ClientPlanModelView>()
            .ForMember(d => d.ClientName, o => o.MapFrom(s => s.Client.Name))
            .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan.Name));
            //

            // General City
            cfg.CreateMap<GeneralCityModelView, GeneralCity>();
            cfg.CreateMap<GeneralCity, GeneralCityModelView>()
            .ForMember(d => d.GeneralCountryId, o => o.MapFrom(s => s.GeneralCountry.Name));
            //

            // General Classification
            cfg.CreateMap<GeneralClassificationModelView, GeneralClassification>();
            cfg.CreateMap<GeneralClassification, GeneralClassificationModelView>();
            //

            // General Country
            cfg.CreateMap<GeneralCountryModelView, GeneralCountry>();
            cfg.CreateMap<GeneralCountry, GeneralCountryModelView>();
            //

            // General District
            cfg.CreateMap<GeneralDistrictModelView, GeneralDistrict>();
            cfg.CreateMap<GeneralDistrict, GeneralDistrictModelView>()
            .ForMember(d => d.GeneralCountryName, o => o.MapFrom(s => s.GeneralCountry.Name))
            .ForMember(d => d.GeneralCityName, o => o.MapFrom(s => s.GeneralCity.Name));
            //

            // General Product
            cfg.CreateMap<GeneralProductModelView, GeneralProduct>();
            cfg.CreateMap<GeneralProduct, GeneralProductModelView>()
            .ForMember(d => d.GeneralClassificationName, o => o.MapFrom(s => s.GeneralClassification.Name))
            .ForMember(d => d.GeneralProductUnitList, o => o.MapFrom(s => s.GeneralProductUnits))
            .ForMember(d => d.GeneralProductRecipeList, o => o.MapFrom(s => s.GeneralProductRecipes))
            .ForMember(d => d.GeneralProductPropertyElementList, o => o.MapFrom(s => s.GeneralProductPropertyElements));
            //

            // General Product Property Element
            cfg.CreateMap<GeneralProductPropertyElementModelView, GeneralProductPropertyElement>();
            cfg.CreateMap<GeneralProductPropertyElement, GeneralProductPropertyElementModelView>()
            .ForMember(d => d.GeneralProductName, o => o.MapFrom(s => s.GeneralProduct.Name))
            .ForMember(d => d.GeneralPropertyName, o => o.MapFrom(s => s.GeneralProperty.Name))
            .ForMember(d => d.GeneralPropertyElementName, o => o.MapFrom(s => s.GeneralPropertyElement.Name));
            //


            // General Product Recipe
            cfg.CreateMap<GeneralProductRecipeModelView, GeneralProductRecipe>();
            cfg.CreateMap<GeneralProductRecipe, GeneralProductRecipeModelView>();
            //

            // General Product Unit
            cfg.CreateMap<GeneralProductUnitModelView, GeneralProductUnit>();
            cfg.CreateMap<GeneralProductUnit, GeneralProductUnitModelView>()
            .ForMember(d => d.GeneralProductName, o => o.MapFrom(s => s.GeneralProduct.Name))
            .ForMember(d => d.GeneralUnitName, o => o.MapFrom(s => s.GeneralUnit.Name));
            //

            // General Property Element
            cfg.CreateMap<GeneralPropertyElementModelView, GeneralPropertyElement>();
            cfg.CreateMap<GeneralPropertyElement, GeneralPropertyElementModelView>()
            .ForMember(d => d.GeneralPropertyName, o => o.MapFrom(s => s.GeneralProperty.Name));
            //

            // General Property
            cfg.CreateMap<GeneralPropertyModelView, GeneralProperty>();
            cfg.CreateMap<GeneralProperty, GeneralPropertyModelView>()
            .ForMember(d => d.GeneralPropertyElementList, o => o.MapFrom(s => s.GeneralPropertyElements));
            //

            // General Unit
            cfg.CreateMap<GeneralUnitModelView, GeneralUnit>();
            cfg.CreateMap<GeneralUnit, GeneralUnitModelView>();
            //

            // Login User
            cfg.CreateMap<LoginUserModelView, LoginUser>()
            .ForMember(d => d.Password, o => o.MapFrom(s => s.Password));
            cfg.CreateMap<LoginUser, LoginUserModelView>()
            .ForMember(d => d.ClientName, o => o.MapFrom(s => s.Client.Name))
            .ForMember(d => d.Schema, o => o.MapFrom(s => s.Client.DbSchema))
             .ForMember(d => d.Password, o => o.MapFrom(s => s.Password));
            //

            // Nationality
            cfg.CreateMap<NationalityModelView, Nationality>();
            cfg.CreateMap<Nationality, NationalityModelView>();
            //

            // Plan Element
            cfg.CreateMap<PlanElementModelView, PlanElement>();
            cfg.CreateMap<PlanElement, PlanElementModelView>()
            .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan.Name));
            //

            // Plan
            cfg.CreateMap<PlanModelView, Plan>();
            cfg.CreateMap<Plan, PlanModelView>()
            .ForMember(d => d.PlanTypeName, o => o.MapFrom(s => s.PlanType.Name))
            .ForMember(d => d.PlanElementList, o => o.MapFrom(s => s.PlanElements));
            //

            // Plan Type
            cfg.CreateMap<PlanTypeModelView, PlanType>();
            cfg.CreateMap<PlanType, PlanTypeModelView>();
            //

            // Request
            cfg.CreateMap<RequestModelView, Request>();
            cfg.CreateMap<Request, RequestModelView>();
            //

            // Type Activity
            cfg.CreateMap<TypeActivityModelView, TypeActivity>();
            cfg.CreateMap<TypeActivity, TypeActivityModelView>();
            //
        }

        public void OrgConfig(Profile cfg)
        {
            // Account Bank
            cfg.CreateMap<AccountBankModelView, AccountBank>();
            cfg.CreateMap<AccountBank, AccountBankModelView>()
            .ForMember(d => d.BankName, o => o.MapFrom(s => s.Bank.Name))
            .ForMember(d => d.AccountName, o => o.MapFrom(s => s.Account.Name))
            .ForMember(d => d.BankBranchName, o => o.MapFrom(s => s.BankBranch.Name));
            //

            // Account
            cfg.CreateMap<AccountModelView, Account>();
            cfg.CreateMap<Account, AccountModelView>()
            .ForMember(d => d.AccountTypeName, o => o.MapFrom(s => s.AccountType.Name));
            //

            // Account Type
            cfg.CreateMap<AccountTypeModelView, AccountType>();
            cfg.CreateMap<AccountType, AccountTypeModelView>();
            //

            // Bank Branch
            cfg.CreateMap<BankBranchModelView, BankBranch>();
            cfg.CreateMap<BankBranch, BankBranchModelView>()
            .ForMember(d => d.BankName, o => o.MapFrom(s => s.Bank.Name))
            .ForMember(d => d.CountryName, o => o.MapFrom(s => s.Country.Name))
            .ForMember(d => d.CityName, o => o.MapFrom(s => s.City.Name))
            .ForMember(d => d.DistrictName, o => o.MapFrom(s => s.District.Name));
            //

            // Bank
            cfg.CreateMap<BankModelView, Bank>();
            cfg.CreateMap<Bank, BankModelView>();
            //

            // Branch
            cfg.CreateMap<BranchModelView, Branch>();
            cfg.CreateMap<Branch, BranchModelView>();
            //

            // City
            cfg.CreateMap<CityModelView, City>();
            cfg.CreateMap<City, CityModelView>()
            .ForMember(d => d.CountryName, o => o.MapFrom(s => s.Country.Name));
            //

            // Classification
            cfg.CreateMap<ClassificationModelView, Classification>();
            cfg.CreateMap<Classification, ClassificationModelView>();
            //

            // Company Profile
            cfg.CreateMap<CompanyProfileModelView, CompanyProfile>();
            cfg.CreateMap<CompanyProfile, CompanyProfileModelView>();
            //

            // Country
            cfg.CreateMap<CountryModelView, Country>();
            cfg.CreateMap<Country, CountryModelView>();
            //

            // Currency
            cfg.CreateMap<CurrencyModelView, Currency>();
            cfg.CreateMap<Currency, CurrencyModelView>();
            //

            // Dealer Group
            cfg.CreateMap<DealerGroupModelView, DealerGroup>();
            cfg.CreateMap<DealerGroup, DealerGroupModelView>();
            //

            // Dealer
            cfg.CreateMap<DealerModelView, Dealer>();
            cfg.CreateMap<Dealer, DealerModelView>();
            //

            // District
            cfg.CreateMap<DistrictModelView, District>();
            cfg.CreateMap<District, DistrictModelView>()
             .ForMember(d => d.CountryName, o => o.MapFrom(s => s.Country.Name))
            .ForMember(d => d.CityName, o => o.MapFrom(s => s.City.Name));
            //

            // Financial Invoice
            cfg.CreateMap<FinancialInvoiceModelView, FinancialInvoice>();
            cfg.CreateMap<FinancialInvoice, FinancialInvoiceModelView>()
            .ForMember(d => d.Net, o => o.MapFrom(s => s.Amount));
            //

            // Financial
            cfg.CreateMap<FinancialModelView, Financial>()
            .ForMember(d => d.FinancialInvoices, o => o.MapFrom(s => s.FinancialInvoiceList));
            cfg.CreateMap<Financial, FinancialModelView>()
            .ForMember(d => d.DealerName, o => o.MapFrom(s => s.Dealer.Name))
            .ForMember(d => d.PaymentTypeName, o => o.MapFrom(s => s.PaymentType.Name))
            .ForMember(d => d.OutlayName, o => o.MapFrom(s => s.Outlay.Name))
            .ForMember(d => d.SafeName, o => o.MapFrom(s => s.Safe.Name))
            .ForMember(d => d.CurrencyName, o => o.MapFrom(s => s.Currency.Name))
            .ForMember(d => d.FinancialInvoiceList, o => o.MapFrom(s => s.FinancialInvoices))
            .ForMember(d => d.FinancialInvoices, o => o.MapFrom(s => s.FinancialInvoices));
            //

            // Financial Type
            cfg.CreateMap<FinancialTypeModelView, FinancialType>();
            cfg.CreateMap<FinancialType, FinancialTypeModelView>();
            //

            // Inventory
            cfg.CreateMap<InventoryModelView, Inventory>();
            cfg.CreateMap<Inventory, InventoryModelView>()
            .ForMember(d => d.StockName, o => o.MapFrom(s => s.Stock.Name))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.Name))
            .ForMember(d => d.BranchName, o => o.MapFrom(s => s.Branch.Name))
            .ForMember(d => d.CreateUserName, o => o.MapFrom(s => s.CreateUser.Name))
            .ForMember(d => d.ModifyUserName, o => o.MapFrom(s => s.ModifyUser.Name))
            .ForMember(d => d.ShiftName, o => o.MapFrom(s => s.Shift.Name))
            .ForMember(d => d.InventoryProductList, o => o.MapFrom(s => s.InventoryProducts));
            //

            // Inventory Product
            cfg.CreateMap<InventoryProductModelView, InventoryProduct>();
            cfg.CreateMap<InventoryProduct, InventoryProductModelView>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.UnitName, o => o.MapFrom(s => s.Unit.Name));
            //

            // Invoice
            cfg.CreateMap<InvoiceModelView, Invoice>()
            .ForMember(d => d.InvoiceProducts, o => o.MapFrom(s => s.InvoiceProductList));
            cfg.CreateMap<Invoice, InvoiceModelView>()
            .ForMember(d => d.DealerName, o => o.MapFrom(s => s.Dealer.Name))
            .ForMember(d => d.PaymentTypeName, o => o.MapFrom(s => s.PaymentType.Name))
            .ForMember(d => d.StockName, o => o.MapFrom(s => s.Stock.Name))
            .ForMember(d => d.CurrencyName, o => o.MapFrom(s => s.Currency.Name))
            .ForMember(d => d.BranchName, o => o.MapFrom(s => s.Branch.Name))
            .ForMember(d => d.CreateUserName, o => o.MapFrom(s => s.CreateUser.Name))
            .ForMember(d => d.ModifyUserName, o => o.MapFrom(s => s.ModifyUser.Name))
            .ForMember(d => d.InvoiceProductList, o => o.MapFrom(s => s.InvoiceProducts));
            //


            // Invoice Product
            cfg.CreateMap<InvoiceProductModelView, InvoiceProduct>();
            cfg.CreateMap<InvoiceProduct, InvoiceProductModelView>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.UnitName, o => o.MapFrom(s => s.Unit.Name))
            .ForMember(d => d.StockName, o => o.MapFrom(s => s.Stock.Name));
            //

            // Transaction Integration
            cfg.CreateMap<TransactionModelView,InvoiceModelView >()
            .ForMember(d => d.InvoiceProductList, o => o.MapFrom(s => s.TransactionProductList));
            cfg.CreateMap<InvoiceModelView, TransactionModelView>()
            .ForMember(d => d.TransactionProductList, o => o.MapFrom(s => s.InvoiceProductList));
            cfg.CreateMap<TransactionProductModelView, InvoiceProductModelView>()
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Cost));
            cfg.CreateMap<InvoiceProductModelView, TransactionProductModelView>()
            .ForMember(d => d.Cost, o => o.MapFrom(s => s.Price));
            //

            // Financial Integration
            cfg.CreateMap<FinancialModelView, InvoiceModelView>();
            cfg.CreateMap<InvoiceModelView, FinancialModelView>();           
            cfg.CreateMap<FinancialInvoiceModelView, InvoiceModelView>();            
            cfg.CreateMap<InvoiceModelView, FinancialInvoiceModelView>()
             .ForMember(d => d.InvoiceId, o => o.MapFrom(s => s.Id));
            //

            // Order Integration           
            cfg.CreateMap<Order, Invoice>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.InvoiceId))
            .ForMember(d => d.InvoiceProducts, o => o.MapFrom(s => s.OrderProducts));
            //

            // Invoice Type
            cfg.CreateMap<InvoiceTypeModelView, InvoiceType>();
            cfg.CreateMap<InvoiceType, InvoiceTypeModelView>();
            //

            // Journal Item
            cfg.CreateMap<JournalItemModelView, JournalItem>();
            cfg.CreateMap<JournalItem, JournalItemModelView>()
            .ForMember(d => d.JournalCode, o => o.MapFrom(s => s.Journal.Code))
            .ForMember(d => d.AccountName, o => o.MapFrom(s => s.Account.Code));
            //

            // Journal
            cfg.CreateMap<JournalModelView, Journal>();
            cfg.CreateMap<Journal, JournalModelView>()
            .ForMember(d => d.CurrencyName, o => o.MapFrom(s => s.Currency.Code));
            //

            // LogSys
            cfg.CreateMap<LogSysModelView, LogSys>();
            cfg.CreateMap<LogSys, LogSysModelView>();
            //

            // Notification
            cfg.CreateMap<NotificationModelView, Notification>();
            cfg.CreateMap<Notification, NotificationModelView>();
            //
          
            // Order
            cfg.CreateMap<OrderModelView, Order>();
            cfg.CreateMap<Order, OrderModelView>()
            .ForMember(d => d.TableName, o => o.MapFrom(s => s.Table.Name))
            .ForMember(d => d.DealerName, o => o.MapFrom(s => s.Dealer.Name))
            .ForMember(d => d.BranchName, o => o.MapFrom(s => s.Branch.Name))
            .ForMember(d => d.CreateUserName, o => o.MapFrom(s => s.CreateUser.Name))
            .ForMember(d => d.ModifyUserName, o => o.MapFrom(s => s.ModifyUser.Name))
            .ForMember(d => d.OrderProductList, o => o.MapFrom(s => s.OrderProducts));
            //

            // Order Product
            cfg.CreateMap<OrderProductModelView, OrderProduct>();
            cfg.CreateMap<OrderProduct, OrderProductModelView>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Code))
            .ForMember(d => d.UnitName, o => o.MapFrom(s => s.Unit.Code));
            //

            // Order Type
            cfg.CreateMap<OrderTypeModelView, OrderType>();
            cfg.CreateMap<OrderType, OrderTypeModelView>();
            //

            // Outlay
            cfg.CreateMap<OutlayModelView, Outlay>();
            cfg.CreateMap<Outlay, OutlayModelView>();
            //

            // Payment Type
            cfg.CreateMap<PaymentTypeModelView, PaymentType>();
            cfg.CreateMap<PaymentType, PaymentTypeModelView>();
            //

            // Permission
            cfg.CreateMap<PermissionModelView, Permission>();
            cfg.CreateMap<Permission, PermissionModelView>();
            //

            // Preference
            cfg.CreateMap<PreferenceModelView, Preference>();
            cfg.CreateMap<Preference, PreferenceModelView>();
            //

            // Product
            cfg.CreateMap<ProductModelView, Product>();
            cfg.CreateMap<Product, ProductModelView>()
            .ForMember(d => d.ClassificationName, o => o.MapFrom(s => s.Classification.Name))
            .ForMember(d => d.DealerName, o => o.MapFrom(s => s.Dealer.Name))
            .ForMember(d => d.ProductUnitList, o => o.MapFrom(s => s.ProductUnits))
            .ForMember(d => d.ProductRecipeList, o => o.MapFrom(s => s.ProductRecipes))
            .ForMember(d => d.ProductPropertyElementList, o => o.MapFrom(s => s.ProductPropertyElements));
            //

            // Product Property Element
            cfg.CreateMap<ProductPropertyElementModelView, ProductPropertyElement>();
            cfg.CreateMap<ProductPropertyElement, ProductPropertyElementModelView>();
            //

            // Product Recipe
            cfg.CreateMap<ProductRecipeModelView, ProductRecipe>();
            cfg.CreateMap<ProductRecipe, ProductRecipeModelView>();
            //

            // Product Unit
            cfg.CreateMap<ProductUnitModelView, ProductUnit>();
            cfg.CreateMap<ProductUnit, ProductUnitModelView>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Code))
            .ForMember(d => d.UnitName, o => o.MapFrom(s => s.Unit.Code));
            //

            // Property Element
            cfg.CreateMap<PropertyElementModelView, PropertyElement>();
            cfg.CreateMap<PropertyElement, PropertyElementModelView>()
            .ForMember(d => d.PropertyName, o => o.MapFrom(s => s.Property.Code));
            //

            // Property
            cfg.CreateMap<PropertyModelView, Property>();
            cfg.CreateMap<Property, PropertyModelView>()
            .ForMember(d => d.PropertyElementList, o => o.MapFrom(s => s.PropertyElements));
            //

            // Role
            cfg.CreateMap<RoleModelView, Role>();
            cfg.CreateMap<Role, RoleModelView>();
            //

            // Role Permission
            cfg.CreateMap<RolePermissionModelView, RolePermission>();
            cfg.CreateMap<RolePermission, RolePermissionModelView>()
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name))
            .ForMember(d => d.PermissionName, o => o.MapFrom(s => s.Permission.Name));
            //

            // Safe
            cfg.CreateMap<SafeModelView, Safe>();
            cfg.CreateMap<Safe, SafeModelView>();
            //

            // Shift
            cfg.CreateMap<ShiftModelView, Shift>();
            cfg.CreateMap<Shift, ShiftModelView>();
            //

            // Stock
            cfg.CreateMap<StockModelView, Stock>();
            cfg.CreateMap<Stock, StockModelView>()
            .ForMember(d => d.BranchName, o => o.MapFrom(s => s.Branch.Name));
            //

            // Table
            cfg.CreateMap<TableModelView, Table>();
            cfg.CreateMap<Table, TableModelView>();
            //

            // Transaction
            cfg.CreateMap<TransactionModelView, Transaction>()
            .ForMember(d => d.TransactionProducts, o => o.MapFrom(s => s.TransactionProductList));
            cfg.CreateMap<Transaction, TransactionModelView>()
            .ForMember(d => d.DealerName, o => o.MapFrom(s => s.Dealer.Name))
            .ForMember(d => d.StockName, o => o.MapFrom(s => s.Stock.Name))
            .ForMember(d => d.ToStockName, o => o.MapFrom(s => s.ToStock.Name))
            .ForMember(d => d.BranchName, o => o.MapFrom(s => s.Branch.Name))
            .ForMember(d => d.CreateUserName, o => o.MapFrom(s => s.CreateUser.Name))
            .ForMember(d => d.ModifyUserName, o => o.MapFrom(s => s.ModifyUser.Name))
            .ForMember(d => d.TransactionProductList, o => o.MapFrom(s => s.TransactionProducts))
            .ForMember(d => d.TransactionProducts, o => o.MapFrom(s => s.TransactionProducts));
            //

            // Transaction Product
            cfg.CreateMap<TransactionProductModelView, TransactionProduct>();
            cfg.CreateMap<TransactionProduct, TransactionProductModelView>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.UnitName, o => o.MapFrom(s => s.Unit.Name))
            .ForMember(d => d.StockName, o => o.MapFrom(s => s.Stock.Name));
            //

            // Transaction Type
            cfg.CreateMap<TransactionTypeModelView, TransactionType>();
            cfg.CreateMap<TransactionType, TransactionTypeModelView>();
            //

            // Unit
            cfg.CreateMap<UnitModelView, Unit>();
            cfg.CreateMap<Unit, UnitModelView>();
            //

            // User
            cfg.CreateMap<UserModelView, User>();
            cfg.CreateMap<User, UserModelView>()
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name))
            .ForMember(d => d.BranchName, o => o.MapFrom(s => s.Branch.Name))
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name));
            //
        }
    }
}