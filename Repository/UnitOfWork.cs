using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class UnitOfWork
    {
        private UnitRepo _unitRepo;
        public UnitRepo unitRepo
        {
            get
            {

                if (this._unitRepo == null)
                    this._unitRepo = new UnitRepo();
                
                return _unitRepo;
            }
        }

        private DealerRepo _dealerRepo;
        public DealerRepo dealerRepo
        {
            get
            {

                if (this._dealerRepo == null)
                    this._dealerRepo = new DealerRepo();

                return _dealerRepo;
            }
        }

        private ClassificationRepo _classificationRepo;
        public ClassificationRepo classificationRepo
        {
            get
            {

                if (this._classificationRepo == null)
                    this._classificationRepo = new ClassificationRepo();

                return _classificationRepo;
            }
        }

        private ProductRepo _productRepo;
        public ProductRepo productRepo
        {
            get
            {

                if (this._productRepo == null)
                    this._productRepo = new ProductRepo();

                return _productRepo;
            }
        }

        private ProductUnitRepo _productUnitRepo;
        public ProductUnitRepo productUnitRepo
        {
            get
            {

                if (this._productUnitRepo == null)
                    this._productUnitRepo = new ProductUnitRepo();

                return _productUnitRepo;
            }
        }

        private BranchRepo _branchRepo;
        public BranchRepo branchRepo
        {
            get
            {

                if (this._branchRepo == null)
                    this._branchRepo = new BranchRepo();

                return _branchRepo;
            }
        }

        private StoreRepo _storeRepo;
        public StoreRepo storeRepo
        {
            get
            {

                if (this._storeRepo == null)
                    this._storeRepo = new StoreRepo();

                return _storeRepo;
            }
        }

        private RolechRepo _roleRepo;
        public RolechRepo roleRepo
        {
            get
            {

                if (this._roleRepo == null)
                    this._roleRepo = new RolechRepo();

                return _roleRepo;
            }
        }

        private ShiftRepo _shiftRepo;
        public ShiftRepo shiftRepo
        {
            get
            {

                if (this._shiftRepo == null)
                    this._shiftRepo = new ShiftRepo();

                return _shiftRepo;
            }
        }

        private UserRepo _userRepo;
        public UserRepo userRepo
        {
            get
            {

                if (this._userRepo == null)
                    this._userRepo = new UserRepo();

                return _userRepo;
            }
        }

        private PermissionRepo _permissionRepo;
        public PermissionRepo permissionRepo
        {
            get
            {

                if (this._permissionRepo == null)
                    this._permissionRepo = new PermissionRepo();

                return _permissionRepo;
            }
        }

        private InvoiceRepo _invoiceRepo;
        public InvoiceRepo invoiceRepo
        {
            get
            {

                if (this._invoiceRepo == null)
                    this._invoiceRepo = new InvoiceRepo();

                return _invoiceRepo;
            }
        }

        private PaymentTypeRepo _paymentTypeRepo;
        public PaymentTypeRepo paymentTypeRepo
        {
            get
            {

                if (this._paymentTypeRepo == null)
                    this._paymentTypeRepo = new PaymentTypeRepo();

                return _paymentTypeRepo;
            }
        }

        private InvoiceProductRepo _invoiceProductRepo;
        public InvoiceProductRepo invoiceProductRepo
        {
            get
            {

                if (this._invoiceProductRepo == null)
                    this._invoiceProductRepo = new InvoiceProductRepo();

                return _invoiceProductRepo;
            }
        }
        
    }
}