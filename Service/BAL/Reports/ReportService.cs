using X.PagedList;
using Repository;
using System;
using Entity.ModelReport;

namespace Service
{
    public class ReportService 
    {
        UnitOfWork repo;
        public ReportService()
        {
            if (repo == null)
                repo = new UnitOfWork();
        }
     
        public IPagedList<InvoiceDetail> InvoiceDetail(long typeId, DateTime fromDate, DateTime toDate, long dealerId, long shiftId, long branchId, long userId , int page = 1 , int pageSize = 100)
        {
            var obList = repo.reportRepo.InvoiceDetails(typeId , fromDate , toDate , dealerId , shiftId , branchId , userId).ToPagedList(page, pageSize);
            foreach (var ob in obList)
            {

            }
            return obList;
        }
    }
}