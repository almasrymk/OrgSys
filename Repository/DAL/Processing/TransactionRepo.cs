using Domain.Entities;
using Domain.Enums;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class TransactionRepo : CurdOrg<Transaction>
    {
        public TransactionRepo(string Schema) : base(Schema) { }

        public override IQueryable<Transaction> GetList(Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy, string includeProperties = "", Status status = Status.New)
        {
            return base.GetList(a => a.OrderByDescending(e => e.Id), includeProperties, status);
        }

        public override IQueryable<Transaction> GetList(Expression<Func<Transaction, bool>> filter, Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy, string includeProperties = "", Status status = Status.New)
        {
            return base.GetList(filter, a => a.OrderByDescending(e => e.Id), includeProperties, status);
        }
    }
}