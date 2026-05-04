using Entity.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using Utility;

namespace Repository
{
    public class TransactionRepo : CurdOrg<Transaction>
    {
        public TransactionRepo(string Schema) : base(Schema) { }

        public override IQueryable<Transaction> GetList(Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy, string includeProperties = "", Status status = Status.All)
        {
            return base.GetList(a => a.OrderByDescending(e => e.Id), includeProperties, status);
        }

        public override IQueryable<Transaction> GetList(Expression<Func<Transaction, bool>> filter, Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy, string includeProperties = "", Status status = Status.All)
        {
            return base.GetList(filter, a => a.OrderByDescending(e => e.Id), includeProperties, status);
        }
    }
}