using Entity.Model;
using Entity.ModelView;
using X.PagedList;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.BAL
{
    public class PaymentTypeService : BaseService<PaymentTypeModelView>
    {
        UnitOfWork repo;
        public PaymentTypeService()
        {
            repo = new UnitOfWork();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public PaymentTypeModelView Save(PaymentTypeModelView ob)
        {
            return new PaymentTypeModelView(repo.paymentTypeRepo.AddOrUpdate(ob.Model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            return repo.paymentTypeRepo.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PaymentTypeModelView> GetAll(long parentId = 0, long TypeId = 0)
        {
            return repo.paymentTypeRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PaymentTypeModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<PaymentTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0)
        {
            return repo.paymentTypeRepo.GetList(e => e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PaymentTypeModelView(e)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<PaymentTypeModelView> GetAll(long parentId = 0, long TypeId = 0 ,int page = 1, int pageSize = 20)
        {
            return repo.paymentTypeRepo.GetList(e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PaymentTypeModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<PaymentTypeModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20)
        {
            return repo.paymentTypeRepo.GetList(e => "" + textSearch == "" || e.Name.Contains("" + textSearch), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PaymentTypeModelView(e)).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PaymentTypeModelView Get(long Id)
        {
            return new PaymentTypeModelView(repo.paymentTypeRepo.Get(e => e.Id == Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public PaymentTypeModelView Get(string textSearch)
        {
            return new PaymentTypeModelView(repo.paymentTypeRepo.Get(e => e.Name.Contains("" + textSearch)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(List<long> ids)
        {
            return repo.paymentTypeRepo.Delete(ids);
        }

        public List<PaymentTypeModelView> GetAll(List<long> ids, long TypeId = 0)
        {
            return repo.paymentTypeRepo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), "", Utility.Status.New).Select(e => new PaymentTypeModelView(e)).ToList();
        }
    }
}