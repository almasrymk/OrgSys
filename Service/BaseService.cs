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

        List<entity> GetAll();

        List<entity> GetAll(string textSearch);

        IPagedList<entity> GetAll(int page, int pageSize);

        IPagedList<entity> GetAll(string textSearch , int page, int pageSize);
    }
}
