using System;
using Entity;
using Utility;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CurdAdmin<entity> : ICurd<entity> where entity : BaseModel
    {
        public AdminContext db;

        public CurdAdmin()
        {
            if (this.db == null)
                this.db = new AdminContext(new DbContextOptions<AdminContext>());
        }

        public virtual long GetMaXCode(Func<entity, bool> filter = null)
        {
            IQueryable<entity> query = db.Set<entity>();
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
       
        public virtual entity Get(Func<entity, bool> filter = null, string includeProperties = "")
        {
            IQueryable<entity> query = db.Set<entity>();
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            var ob = query.FirstOrDefault(filter);
            if (ob == null)
                ob = Activator.CreateInstance<entity>();
            return ob;
        }

        public virtual IQueryable<entity> GetList(Func<IQueryable<entity>, IOrderedQueryable<entity>> orderBy, string includeProperties = "", Status status = Status.All)
        {
            IQueryable<entity> query = db.Set<entity>();

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query.Where(e => (e.Status != Status.Deleted || status == Status.All) && e.Hide != true));               
            }
            else
            {
                return query.Where(e => (e.Status != Status.Deleted || status == Status.All) && e.Hide != true);
            }
        }

        public virtual IQueryable<entity> GetList(Expression<Func<entity, bool>> filter, Func<IQueryable<entity>, IOrderedQueryable<entity>> orderBy, string includeProperties = "", Status status = Status.All)
        {
            IQueryable<entity> query = db.Set<entity>();

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
                return orderBy(query.Where(e => (e.Status != Status.Deleted || status == Status.All) && e.Hide != true));
            }
            else
            {
                return query.Where(e => (e.Status != Status.Deleted || status == Status.All) && e.Hide != true);
            }
        }

        public virtual IQueryable<entity> GetList(Expression<Func<entity, bool>> filter, string includeProperties = "", Status status = Status.All)
        {
            IQueryable<entity> query = db.Set<entity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.Where(e => (e.Status != Status.Deleted || status == Status.All) && e.Hide != true);
        }

        public virtual entity AddOrUpdate(entity ob)
        {
            if (ob.Id == 0)
                db.Set<entity>().Add(ob);
            else
                db.Entry<entity>(db.Set<entity>().Find(ob.Id)).CurrentValues.SetValues(ob);
            db.SaveChanges();           
            return ob;
        }

        public virtual entity AddOrUpdateTemp(entity ob)
        {
            if (ob.Id == 0)
                db.Set<entity>().Add(ob);
            else
                db.Entry<entity>(db.Set<entity>().Find(ob.Id)).CurrentValues.SetValues(ob);
            return ob;
        }

        public virtual bool SaveChanges()
        {
            return db.SaveChanges() > -1;
        }

        public virtual bool Delete(long Id)
        {
            var ob = db.Set<entity>().Find(Id);
            if (ob == null || ob.Id == 0)
                return false;
            ob.ImgPath = null;
            ob.Status = Status.Deleted;
            db.Entry<entity>(db.Set<entity>().Find(ob.Id)).CurrentValues.SetValues(ob);
            db.SaveChanges();
            return true;
        }

        public virtual bool ShiftDelete(long Id)
        {
            var ob = db.Set<entity>().Find(Id);
            if (ob == null || ob.Id == 0)
                return false;
            db.Set<entity>().Remove(ob);
            db.SaveChanges();
            return true;
        }

        public virtual bool Delete(List<long> Ids)
        {
            var obs = db.Set<entity>().Where(e => Ids.Contains(e.Id)).ToList();
            foreach (var ob in obs)
            {
                ob.ImgPath = null;
                ob.Status = Status.Deleted;
                db.Entry<entity>(db.Set<entity>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            db.SaveChanges();
            return true;
        }

        public virtual bool ShiftDelete(List<long> Ids)
        {
            var obs = db.Set<entity>().Where(e => Ids.Contains(e.Id)).ToList();
            if (obs != null && obs.Count > 0)
            {
                db.Set<entity>().RemoveRange(obs);
                db.SaveChanges();
            }
            return true;
        }

        public virtual bool Any(Func<entity, bool> filter = null)
        {
            IQueryable<entity> query = db.Set<entity>();

            return query.Any(filter);
        }
    }
}