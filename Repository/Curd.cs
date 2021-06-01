using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Utility;

namespace Repository
{
    public class Curd<entity> where entity : BaseModel
    {
       public OrgContext db;

        public Curd()
        {
            if (this.db == null)
                this.db = new OrgContext(new DbContextOptions<OrgContext>());            
        }

        /// <summary>
        /// Get single object from database
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual entity Get(Func<entity, bool> filter, string includeProperties = "")
        {
            IQueryable<entity> query = db.Set<entity>();
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefault(filter);
        }

        /// <summary>
        /// Get all objects from database
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="status"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get list of objects from database
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="status"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add/Update object into dataabse
        /// </summary>
        /// <param name="ob">Object</param>
        /// <returns>Object</returns>
        public virtual entity AddOrUpdate(entity ob)
        {
            if (ob.Id == 0)
                db.Set<entity>().Add(ob);
            else
                db.Entry<entity>(db.Set<entity>().Find(ob.Id)).CurrentValues.SetValues(ob);
            db.SaveChanges();
            return ob;
        }

        /// <summary>
        /// Delete object from database
        /// </summary>
        /// <param name="Id">Long</param>
        /// <returns>bool</returns>
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

        /// <summary>
        /// Delete object from database
        /// </summary>
        /// <param name="Id">Long</param>
        /// <returns>bool</returns>
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
            if   (obs!= null && obs.Count > 0)
                db.Set<entity>().RemoveRange(obs);
            db.SaveChanges();
            return true;
        }
    }
}