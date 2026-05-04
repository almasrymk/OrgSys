namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.DealerGroup.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using Service;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class DealerGroupController(IConfiguration configuration, IMapper mapper) : MainController<DealerGroupModelView, CreateDealerGroupCommand, UpdateDealerGroupCommand>(configuration, mapper)
    {
        public override async Task<DealerGroupModelView> InitializeData(DealerGroupModelView ob)
        {
            if (ob == null)
                ob = new DealerGroupModelView();
            if (ob.Id == 0)
            {
                ob.CodeNumber = long.Parse("0" + await GetValueApi<DealerGroupModelView>($"GetMax?TypeId={ob.TypeId}")) + 1;
                ob.Code = "" + ob.CodeNumber;
            }
            return ob;
        }

        public async Task<JsonResult> GetList(string txtSearch = "", long TypeId = 0, int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();
            long TypeDealerId = TypeId == 1 || TypeId == 3 ? 1 : 2;

            var itemsList = await GetListApi<DealerGroupModelView>(TypeId: TypeDealerId , TextSearch: txtSearch, Page: page, PageSize: pageSize);
            var list = itemsList.Distinct().OrderBy(_ => _.Name)
                .Select(_ => new
                {
                    _.Id,
                    _.Name
                })
                .ToList();
            return Json(list);
        }
    }
}