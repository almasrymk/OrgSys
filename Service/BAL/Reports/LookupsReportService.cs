using Repository;
using X.PagedList;
using Domain.EntitiesReport;
using X.PagedList.Extensions;

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

        public IPagedList<ProductList> GetProducts(long ClassificationId, int page = 1, int pageSize = 100)
        {
            return repo.lookupsRepo.GetProducts(ClassificationId).ToPagedList(page, pageSize);
        }

        public IPagedList<StockList> GetStocks(string txtSearch, int page = 1, int pageSize = 100)
        {
            return repo.lookupsRepo.GetStocks(txtSearch).ToPagedList(page, pageSize);
        }

        public IPagedList<SafeList> GetSafes(string txtSearch, int page = 1, int pageSize = 100)
        {
            return repo.lookupsRepo.GetSafes(txtSearch).ToPagedList(page, pageSize);
        }
    }
}