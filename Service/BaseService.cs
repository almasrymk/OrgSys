using PagedList;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace Service
{
    public interface BaseService<entity> where entity : BaseModel
    {
        entity Save(entity ob);

        bool Delete(long id);

        bool Delete(List<long> ids);

        entity Get(long id);

        entity Get(string textSearch);

        List<entity> GetAll(long parentId =0 , long TypeId = 0);

        List<entity> GetAll(string textSearch , long parentId = 0, long TypeId = 0);

        IPagedList<entity> GetAll(long parentId = 0, long TypeId = 0 , int page = 1, int pageSize = 20);

        IPagedList<entity> GetAll(string textSearch , long parentId = 0, long TypeId = 0, int page = 1, int pageSize = 20);
    }
}
