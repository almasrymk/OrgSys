using System;
using Domain;
using Domain.Entities;
using Utility;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CurdAdmin<Tentity> : ICurd<Tentity> where Tentity : BaseModel
    {
        public AdminContext db;

        public CurdAdmin()
        {
            if (this.db == null)
                this.db = new AdminContext(new DbContextOptions<AdminContext>());
        }

        public virtual long GetMaXCode(Func<Tentity, bool> filter = null)
        {
            IQueryable<Tentity> query = db.Set<Tentity>();
            if (filter != null)
            {
                if (query.Any(filter))
                    return query.Where(filter).Max(e => e.CodeNumber) + 1;
            }
            else
            {
                if (query.Any())
                    return query.Max(e => e.CodeNumber) + 1;
            }
            return 1;
        }
       
        public virtual Tentity Get(Func<Tentity, bool> filter = null, string includeProperties = "")
        {
            IQueryable<Tentity> query = db.Set<Tentity>();
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            var ob = query.FirstOrDefault(filter);
            if (ob == null)
                ob = Activator.CreateInstance<Tentity>();
            return ob;
        }

        public virtual IQueryable<Tentity> GetList(Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>> orderBy, string includeProperties = "", Status status = Status.All)
        {
            IQueryable<Tentity> query = db.Set<Tentity>();

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query.Where(e => (e.Status != Domain.Enums.Status.Deleted || status == Status.All) && e.Hide != true));               
            }
            else
            {
                return query.Where(e => (e.Status != Domain.Enums.Status.Deleted || status == Status.All) && e.Hide != true);
            }
        }

        public virtual IQueryable<Tentity> GetList(Expression<Func<Tentity, bool>> filter, Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>> orderBy, string includeProperties = "", Status status = Status.All)
        {
            IQueryable<Tentity> query = db.Set<Tentity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query.Where(e => (e.Status != Domain.Enums.Status.Deleted || status == Status.All) && e.Hide != true));
            }
            else
            {
                return query.Where(e => (e.Status != Domain.Enums.Status.Deleted || status == Status.All) && e.Hide != true);
            }
        }

        public virtual IQueryable<Tentity> GetList(Expression<Func<Tentity, bool>> filter, string includeProperties = "", Status status = Status.All)
        {
            IQueryable<Tentity> query = db.Set<Tentity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.Where(e => (e.Status != Domain.Enums.Status.Deleted || status == Status.All) && e.Hide != true);
        }

        public virtual Tentity AddOrUpdate(Tentity ob)
        {
            if (ob.Id == 0)
                db.Set<Tentity>().Add(ob);
            else
                db.Entry<Tentity>(db.Set<Tentity>().Find(ob.Id)).CurrentValues.SetValues(ob);
            db.SaveChanges();           
            return ob;
        }

        public virtual Tentity AddOrUpdateTemp(Tentity ob)
        {
            if (ob.Id == 0)
                db.Set<Tentity>().Add(ob);
            else
                db.Entry<Tentity>(db.Set<Tentity>().Find(ob.Id)).CurrentValues.SetValues(ob);
            return ob;
        }

        public virtual bool SaveChanges()
        {
            return db.SaveChanges() > -1;
        }

        public virtual bool Delete(long Id)
        {
            var ob = db.Set<Tentity>().Find(Id);
            if (ob == null || ob.Id == 0)
                return false;
            ob.ImgPath = null;
            ob.Status = Domain.Enums.Status.Deleted;
            db.Entry<Tentity>(db.Set<Tentity>().Find(ob.Id)).CurrentValues.SetValues(ob);
            db.SaveChanges();
            return true;
        }

        public virtual bool ShiftDelete(long Id)
        {
            var ob = db.Set<Tentity>().Find(Id);
            if (ob == null || ob.Id == 0)
                return false;
            db.Set<Tentity>().Remove(ob);
            db.SaveChanges();
            return true;
        }

        public virtual bool Delete(List<long> Ids)
        {
            var obs = db.Set<Tentity>().Where(e => Ids.Contains(e.Id)).ToList();
            foreach (var ob in obs)
            {
                ob.ImgPath = null;
                ob.Status = Domain.Enums.Status.Deleted;
                db.Entry<Tentity>(db.Set<Tentity>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            db.SaveChanges();
            return true;
        }

        public virtual bool ShiftDelete(List<long> Ids)
        {
            var obs = db.Set<Tentity>().Where(e => Ids.Contains(e.Id)).ToList();
            if (obs != null && obs.Count > 0)
            {
                db.Set<Tentity>().RemoveRange(obs);
                db.SaveChanges();
            }
            return true;
        }

        public virtual bool Any(Func<Tentity, bool> filter = null)
        {
            IQueryable<Tentity> query = db.Set<Tentity>();

            return query.Any(filter);
        }
    }
}