using Entity;
using System.Linq;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;

namespace Service
{
    public class UnitService : BaseOrgService<UnitModelView, Unit>
    {
        public UnitService(string Schema) : base(Schema) { }

        public List<UnitModelView> GetAllByProductId(long ProductId = 0)
        {
            var itemUnits = repoAll.productUnitRepo.GetList(e => e.ProductId == ProductId, null, Includes, Utility.Status.New).ToList();
            if (itemUnits == null)
                itemUnits = new List<ProductUnit>();
            List<long> ids = itemUnits.Select(e => e.UnitId).ToList();
            return repo.GetList(e => ids.Contains(e.Id), e => e.OrderBy(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<UnitModelView>()).ToList();
        }
    }
}