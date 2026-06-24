namespace Repository
{
    public class UnitOfWorkAdmin
    {
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
    }
}