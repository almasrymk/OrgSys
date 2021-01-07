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
    }
}
