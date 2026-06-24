//using Entity;
using X.PagedList;
using System.Collections.Generic;
using Domain.Entities;

namespace Service
{
    public interface IBaseService<entityModelView> where entityModelView : BaseModel
    {
        entityModelView Save(entityModelView ob);

        bool Delete(long id);

        bool Delete(List<long> ids);

        entityModelView Get(long Id);

        entityModelView Get(string textSearch);

        List<entityModelView> GetAll(long parentId =0 , long TypeId = 0);

        List<entityModelView> GetAll(List<long> Ids, long TypeId =0);

        List<entityModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0);

        IPagedList<entityModelView> GetAll(long parentId = 0, long TypeId = 0 , int page = 1, int pageSize = 20);

        IPagedList<entityModelView> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20);

        long GetMaxCode(long type = 0);
    }
}