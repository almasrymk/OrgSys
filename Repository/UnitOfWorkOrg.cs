using Entity;
using System;
using System.Reflection;
using System.Collections.Generic;
using Repository.DAL.Output;

namespace Repository
{
    public class UnitOfWorkOrg
    {
        public string Schema;

        public UnitOfWorkOrg(string _Schema)
        {
            Schema = _Schema;
        }

        #region Input
        #region Organization
        private BranchRepo _branchRepo;
        public BranchRepo branchRepo
        {
            get
            {
                if (this._branchRepo == null)
                    this._branchRepo = new BranchRepo(Schema);

                return _branchRepo;
            }
        }

        private StockRepo _StockRepo;
        public StockRepo StockRepo
        {
            get
            {
                if (this._StockRepo == null)
                    this._StockRepo = new StockRepo(Schema);

                return _StockRepo;
            }
        }

        private ShiftRepo _shiftRepo;
        public ShiftRepo shiftRepo
        {
            get
            {
                if (this._shiftRepo == null)
                    this._shiftRepo = new ShiftRepo(Schema);

                return _shiftRepo;
            }
        }
        #endregion

        #region Permission
        private RoleRepo _roleRepo;
        public RoleRepo roleRepo
        {
            get
            {
                if (this._roleRepo == null)
                    this._roleRepo = new RoleRepo(Schema);

                return _roleRepo;
            }
        }

        private UserRepo _userRepo;
        public UserRepo userRepo
        {
            get
            {
                if (this._userRepo == null)
                    this._userRepo = new UserRepo(Schema);

                return _userRepo;
            }
        }

        private PermissionRepo _permissionRepo;
        public PermissionRepo permissionRepo
        {
            get
            {
                if (this._permissionRepo == null)
                    this._permissionRepo = new PermissionRepo(Schema);

                return _permissionRepo;
            }
        }

        private RolePermissionRepo _rolePermissionRepo;
        public RolePermissionRepo rolePermissionRepo
        {
            get
            {
                if (this._rolePermissionRepo == null)
                    this._rolePermissionRepo = new RolePermissionRepo(Schema);

                return _rolePermissionRepo;
            }
        }
        #endregion

        #region Types
        private OrderTypeRepo _orderTypeRepo;
        public OrderTypeRepo orderTypeRepo
        {
            get
            {
                if (this._orderTypeRepo == null)
                    this._orderTypeRepo = new OrderTypeRepo(Schema);

                return _orderTypeRepo;
            }
        }

        private InvoiceTypeRepo _invoiceTypeRepo;
        public InvoiceTypeRepo invoiceTypeRepo
        {
            get
            {
                if (this._invoiceTypeRepo == null)
                    this._invoiceTypeRepo = new InvoiceTypeRepo(Schema);

                return _invoiceTypeRepo;
            }
        }

        private TransactionTypeRepo _transactionTypeRepo;
        public TransactionTypeRepo transactionTypeRepo
        {
            get
            {
                if (this._transactionTypeRepo == null)
                    this._transactionTypeRepo = new TransactionTypeRepo(Schema);

                return _transactionTypeRepo;
            }
        }

        private FinancialTypeRepo _financialTypeRepo;
        public FinancialTypeRepo financialTypeRepo
        {
            get
            {
                if (this._financialTypeRepo == null)
                    this._financialTypeRepo = new FinancialTypeRepo(Schema);

                return _financialTypeRepo;
            }
        }

        private PaymentTypeRepo _paymentTypeRepo;
        public PaymentTypeRepo paymentTypeRepo
        {
            get
            {
                if (this._paymentTypeRepo == null)
                    this._paymentTypeRepo = new PaymentTypeRepo(Schema);

                return _paymentTypeRepo;
            }
        }
        #endregion

        private UnitRepo _unitRepo;
        public UnitRepo unitRepo
        {
            get
            {
                if (this._unitRepo == null)
                    this._unitRepo = new UnitRepo(Schema);

                return _unitRepo;
            }
        }

        private PropertyRepo _propertyRepo;
        public PropertyRepo propertyRepo
        {
            get
            {
                if (this._propertyRepo == null)
                    this._propertyRepo = new PropertyRepo(Schema);

                return _propertyRepo;
            }
        }

        private ProductPropertyElementRepo _productpropertyelementRepo;
        public ProductPropertyElementRepo productpropertyelementRepo
        {
            get
            {
                if (this._productpropertyelementRepo == null)
                    this._productpropertyelementRepo = new ProductPropertyElementRepo(Schema);

                return _productpropertyelementRepo;
            }
        }

        private DealerRepo _dealerRepo;
        public DealerRepo dealerRepo
        {
            get
            {
                if (this._dealerRepo == null)
                    this._dealerRepo = new DealerRepo(Schema);

                return _dealerRepo;
            }
        }

        private PropertyElementRepo _propertyElementRepo;
        public PropertyElementRepo propertyelementRepo
        {
            get
            {
                if (this._propertyElementRepo == null)
                    this._propertyElementRepo = new PropertyElementRepo(Schema);

                return _propertyElementRepo;
            }
        }

        private ClassificationRepo _classificationRepo;
        public ClassificationRepo classificationRepo
        {
            get
            {
                if (this._classificationRepo == null)
                    this._classificationRepo = new ClassificationRepo(Schema);

                return _classificationRepo;
            }
        }

        private ProductRepo _productRepo;
        public ProductRepo productRepo
        {
            get
            {
                if (this._productRepo == null)
                    this._productRepo = new ProductRepo(Schema);

                return _productRepo;
            }
        }

        private ProductUnitRepo _productUnitRepo;
        public ProductUnitRepo productUnitRepo
        {
            get
            {
                if (this._productUnitRepo == null)
                    this._productUnitRepo = new ProductUnitRepo(Schema);

                return _productUnitRepo;
            }
        }

        private RecipeRepo _recipeRepo;
        public RecipeRepo recipeRepo
        {
            get
            {
                if (this._recipeRepo == null)
                    this._recipeRepo = new RecipeRepo(Schema);

                return _recipeRepo;
            }
        }

        private PreferenceRepo _preferenceRepo;
        public PreferenceRepo preferenceRepo
        {
            get
            {
                if (this._preferenceRepo == null)
                    this._preferenceRepo = new PreferenceRepo(Schema);

                return _preferenceRepo;
            }
        }

        private TableRepo _tableRepo;
        public TableRepo tableRepo
        {
            get
            {
                if (this._tableRepo == null)
                    this._tableRepo = new TableRepo(Schema);

                return _tableRepo;
            }
        }

        private SafeRepo _safeRepo;
        public SafeRepo safeRepo
        {
            get
            {
                if (this._safeRepo == null)
                    this._safeRepo = new SafeRepo(Schema);

                return _safeRepo;
            }
        }

        private OutlayRepo _outlayRepo;
        public OutlayRepo outlayRepo
        {
            get
            {
                if (this._outlayRepo == null)
                    this._outlayRepo = new OutlayRepo(Schema);

                return _outlayRepo;
            }
        }

        private CurrencyRepo _currencyRepo;
        public CurrencyRepo currencyRepo
        {
            get
            {
                if (this._currencyRepo == null)
                    this._currencyRepo = new CurrencyRepo(Schema);

                return _currencyRepo;
            }
        }

        private CompanyProfileRepo _companyProfileRepo;
        public CompanyProfileRepo companyProfileRepo
        {
            get
            {
                if (this._companyProfileRepo == null)
                    this._companyProfileRepo = new CompanyProfileRepo(Schema);

                return _companyProfileRepo;
            }
        }
        #endregion

        #region Processing
        private OrderRepo _orderRepo;
        public OrderRepo orderRepo
        {
            get
            {
                if (this._orderRepo == null)
                    this._orderRepo = new OrderRepo(Schema);

                return _orderRepo;
            }
        }

        private OrderProductRepo _orderProductRepo;
        public OrderProductRepo orderProductRepo
        {
            get
            {
                if (this._orderProductRepo == null)
                    this._orderProductRepo = new OrderProductRepo(Schema);

                return _orderProductRepo;
            }
        }

        private InvoiceRepo _invoiceRepo;
        public InvoiceRepo invoiceRepo
        {
            get
            {
                if (this._invoiceRepo == null)
                    this._invoiceRepo = new InvoiceRepo(Schema);

                return _invoiceRepo;
            }
        }

        private InvoiceProductRepo _invoiceProductRepo;
        public InvoiceProductRepo invoiceProductRepo
        {
            get
            {
                if (this._invoiceProductRepo == null)
                    this._invoiceProductRepo = new InvoiceProductRepo(Schema);

                return _invoiceProductRepo;
            }
        }

        private TransactionRepo _transactionRepo;
        public TransactionRepo transactionRepo
        {
            get
            {
                if (this._transactionRepo == null)
                    this._transactionRepo = new TransactionRepo(Schema);

                return _transactionRepo;
            }
        }

        private TransactionProductRepo _transactionProductRepo;
        public TransactionProductRepo transactionProductRepo
        {
            get
            {
                if (this._transactionProductRepo == null)
                    this._transactionProductRepo = new TransactionProductRepo(Schema);

                return _transactionProductRepo;
            }
        }

        private InventoryRepo _inventoryRepo;
        public InventoryRepo inventoryRepo
        {
            get
            {
                if (this._inventoryRepo == null)
                    this._inventoryRepo = new InventoryRepo(Schema);

                return _inventoryRepo;
            }
        }

        private InventoryProductRepo _inventoryProductRepo;
        public InventoryProductRepo inventoryProductRepo
        {
            get
            {
                if (this._inventoryProductRepo == null)
                    this._inventoryProductRepo = new InventoryProductRepo(Schema);

                return _inventoryProductRepo;
            }
        }

        private FinancialRepo _financialRepo;
        public FinancialRepo financialRepo
        {
            get
            {
                if (this._financialRepo == null)
                    this._financialRepo = new FinancialRepo(Schema);

                return _financialRepo;
            }
        }

        private FinancialInvoiceRepo _financialInvoiceRepo;
        public FinancialInvoiceRepo financialInvoiceRepo
        {
            get
            {
                if (this._financialInvoiceRepo == null)
                    this._financialInvoiceRepo = new FinancialInvoiceRepo(Schema);

                return _financialInvoiceRepo;
            }
        }
        #endregion

        #region Output       
        private LookupsRepo _lookupsRepo;
        public LookupsRepo lookupsRepo
        {
            get
            {
                if (this._lookupsRepo == null)
                    this._lookupsRepo = new LookupsRepo(Schema);

                return _lookupsRepo;
            }
        }

        private DealersStatmentRepo _dealersStatmentRepo;
        public DealersStatmentRepo dealersStatmentRepo
        {
            get
            {
                if (this._dealersStatmentRepo == null)
                    this._dealersStatmentRepo = new DealersStatmentRepo(Schema);

                return _dealersStatmentRepo;
            }
        }

        private DealersBalanceRepo _dealersBalanceRepo;
        public DealersBalanceRepo dealersBalanceRepo
        {
            get
            {
                if (this._dealersBalanceRepo == null)
                    this._dealersBalanceRepo = new DealersBalanceRepo(Schema);

                return _dealersBalanceRepo;
            }
        }

        private WarehousesStatmentRepo _warehousesStatmentRepo;
        public WarehousesStatmentRepo warehousesStatmentRepo
        {
            get
            {
                if (this._warehousesStatmentRepo == null)
                    this._warehousesStatmentRepo = new WarehousesStatmentRepo(Schema);

                return _warehousesStatmentRepo;
            }
        }           
        
        private WarehousesBalanceRepo _warehousesBalanceRepo;
        public WarehousesBalanceRepo warehousesBalanceRepo
        {
            get
            {
                if (this._warehousesBalanceRepo == null)
                    this._warehousesBalanceRepo = new WarehousesBalanceRepo(Schema);

                return _warehousesBalanceRepo;
            }
        }                  
        #endregion
    }
}