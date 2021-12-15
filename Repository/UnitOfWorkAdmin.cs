using Entity;
using Entity.Model;
using Repository.DAL.Input;
using System;
using System.Collections.Generic;

namespace Repository
{
    public class UnitOfWorkAdmin2<Entity> where Entity : BaseModel
    {
        private CurdAdmin<Entity> _db;
        public CurdAdmin<Entity> Db
        {
            get
            {

                if (this._db == null)
                    this._db = new CurdAdmin<Entity>();

                return _db;
            }
        }
    }
    public class UnitOfWorkAdmin
    {             
        #region Admin       
        private RequestRepo _requestRepo;
        public RequestRepo requestRepo
        {
            get
            {

                if (this._requestRepo == null)
                    this._requestRepo = new RequestRepo();

                return _requestRepo;
            }
        }

        private LoginUserRepo _loginUserRepo;
        public LoginUserRepo loginUserRepo
        {
            get
            {

                if (this._loginUserRepo == null)
                    this._loginUserRepo = new LoginUserRepo();

                return _loginUserRepo;
            }
        }

        private ClientRepo _clientRepo;
        public ClientRepo clientRepo
        {
            get
            {

                if (this._clientRepo == null)
                    this._clientRepo = new ClientRepo();

                return _clientRepo;
            }
        }

        private NationalityRepo _nationalityRepo;
        public NationalityRepo nationalityRepo
        {
            get
            {

                if (this._nationalityRepo == null)
                    this._nationalityRepo = new NationalityRepo();

                return _nationalityRepo;
            }
        }

        private TypeActivityRepo _typeActivityRepo;
        public TypeActivityRepo typeActivityRepo
        {
            get
            {

                if (this._typeActivityRepo == null)
                    this._typeActivityRepo = new TypeActivityRepo();

                return _typeActivityRepo;
            }
        }
        #endregion
    }
}