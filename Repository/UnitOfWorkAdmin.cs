using Entity;

namespace Repository
{
    public class UnitOfWorkAdmin<Entity> where Entity : BaseModel
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
}