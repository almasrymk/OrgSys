using System;
using Domain;
using Domain.Entities;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Domain.Enums;

namespace Repository
{
    public interface ICurd<Tentity> where Tentity : BaseModel
    {
        public long GetMaXCode(Func<Tentity, bool> filter = null);

        public Tentity Get(Func<Tentity, bool> filter = null, string includeProperties = "");

        public IQueryable<Tentity> GetList(Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>> orderBy, string includeProperties = "", Status status = Status.New );

        public IQueryable<Tentity> GetList(Expression<Func<Tentity, bool>> filter, Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>> orderBy, string includeProperties = "", Status status = Status.New);

        public IQueryable<Tentity> GetList(Expression<Func<Tentity, bool>> filter, string includeProperties = "", Status status = Status.New);

        public Tentity AddOrUpdate(Tentity ob);

        public Tentity AddOrUpdateTemp(Tentity ob);

        public bool SaveChanges();

        public bool Delete(long Id);

        public bool ShiftDelete(long Id);

        public bool Delete(List<long> Ids);

        public bool ShiftDelete(List<long> Ids);

        public bool Any(Func<Tentity, bool> filter = null);
    }
}