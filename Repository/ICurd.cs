using System;
using Entity;
using Utility;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Repository
{
    public interface ICurd<entity> where entity : BaseModel
    {
        public long GetMaXCode(Func<entity, bool> filter = null);

        public entity Get(Func<entity, bool> filter = null, string includeProperties = "");

        public IQueryable<entity> GetList(Func<IQueryable<entity>, IOrderedQueryable<entity>> orderBy, string includeProperties = "", Status status = Status.All);

        public IQueryable<entity> GetList(Expression<Func<entity, bool>> filter, Func<IQueryable<entity>, IOrderedQueryable<entity>> orderBy, string includeProperties = "", Status status = Status.All);

        public IQueryable<entity> GetList(Expression<Func<entity, bool>> filter, string includeProperties = "", Status status = Status.All);

        public entity AddOrUpdate(entity ob);

        public entity AddOrUpdateTemp(entity ob);

        public bool SaveChanges();

        public bool Delete(long Id);

        public bool ShiftDelete(long Id);

        public bool Delete(List<long> Ids);

        public bool ShiftDelete(List<long> Ids);

        public bool Any(Func<entity, bool> filter = null);
    }
}