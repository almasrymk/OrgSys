using Repository;
using X.PagedList;
using Entity.ModelReport;

namespace Service
{
    public class LookupsReportService
    {
        UnitOfWorkOrg repo;
        private string _Schema;
        public LookupsReportService(string Schema)
        {
            this._Schema = Schema;
            repo = new UnitOfWorkOrg(Schema);
        }

        public IPagedList<DealerList> GetDealers(long typeId, string txtSearch, int page = 1, int pageSize = 100)
        {
            return repo.lookupsRepo.GetDealers(typeId, txtSearch).ToPagedList(page, pageSize);
        }
    }
}