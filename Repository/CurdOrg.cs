using System;
using System.Linq;
using Domain.Entities;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Repository
{
    public class CurdOrg<Tentity> : ICurd<Tentity> where Tentity : BaseModel
    {
        public OrgContext db;
        public CurdOrg(string Schema)
        {
            this.db = new OrgContext(new DbContextOptions<OrgContext>() , Schema);
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
            return query.FirstOrDefault(filter);
        }

        public virtual IQueryable<Tentity> GetList(Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>> orderBy, string includeProperties = "", Status status = Status.New)
        {
            IQueryable<Tentity> query = db.Set<Tentity>();

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query.Where(e => (e.Status != Domain.Enums.Status.Deleted || status == Status.New) && e.Hide != true));
            }
            else
            {
                return query.Where(e => (e.Status != Domain.Enums.Status.Deleted || status == Status.New) && e.Hide != true);
            }
        }

        public virtual IQueryable<Tentity> GetList(Expression<Func<Tentity, bool>> filter, Func<IQueryable<Tentity>, IOrderedQueryable<Tentity>> orderBy, string includeProperties = "", Status status = Status.New)
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
                return orderBy(query.Where(e => (e.Status != Domain.Enums.Status.Deleted || status == Status.New) && e.Hide != true));
            }
            else
            {
                return query.Where(e => (e.Status != Domain.Enums.Status.Deleted || status == Status.New) && e.Hide != true);
            }
        }

        public virtual IQueryable<Tentity> GetList(Expression<Func<Tentity, bool>> filter, string includeProperties = "", Status status = Status.New)
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

            return query.Where(e => (e.Status != Domain.Enums.Status.Deleted || status == Status.New) && e.Hide != true);
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

        //public virtual void AddLog(entity ob , LogType logType , LogAccessLevel logAccessLevel , long EstimateBySecond)
        //{
        //    var user = db.Users.FirstOrDefault(e => e.Id == _UserId);
        //    LogSys logSys = new LogSys();
        //    logSys.CodeNumber = db.LogSys.Max(e => e.CodeNumber) + 1;
        //    logSys.Code = "" + logSys.CodeNumber;
        //    logSys.ResourceId = "" + ob.Id;
        //    logSys.ResourceType = nameof(ob);
        //    logSys.TableName = nameof(ob);
        //    logSys.ScreenName = nameof(ob);
        //    logSys.Date = DateTime.Now;
        //    logSys.LogAccessLevel = logAccessLevel;
        //    logSys.LogType = logType;
        //    logSys.Time = new TimeSpan(DateTime.Now.Hour , DateTime.Now.Minute , DateTime.Now.Second);
        //    logSys.EstimateBySecond = EstimateBySecond;
        //    logSys.UserId = _UserId;

        //    switch (logType)
        //    {
        //        case LogType.Index:
        //            break;
        //        case LogType.View:
        //            break;
        //        case LogType.Add:
        //            logSys.Title = "Add New";
        //            logSys.Message = "Add a new " + nameof(ob) + " by " + user.Name + " on " + DateTime.Now.ToString("dd-MM-yyyy") + " at " + DateTime.Now.Hour;
        //            break;
        //        case LogType.Update:
        //            logSys.Title = "Update";
        //            logSys.Message = "Update " + nameof(ob) + " by " + user.Name + " on " + DateTime.Now.ToString("dd-MM-yyyy") + " at " + DateTime.Now.Hour;
        //            break;
        //        case LogType.Delete:
        //            logSys.Title = "Delete";
        //            logSys.Message = "Delete a " + nameof(ob) + " by " + user.Name + " on " + DateTime.Now.ToString("dd-MM-yyyy") + " at " + DateTime.Now.Hour;
        //            break;
        //        default:
        //            break;
        //    }

        //    db.LogSys.Add(logSys);
        //}
    }
}