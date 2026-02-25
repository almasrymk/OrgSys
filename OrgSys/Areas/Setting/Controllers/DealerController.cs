namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Dealer.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class DealerController(IConfiguration configuration, IMapper mapper) : MainController<DealerModelView, CreateDealerCommand, UpdateDealerCommand>(configuration, mapper)
    {

        public override async Task LoadViewBag(DealerModelView model)
        {
            ViewBag.DealersGroupList = new SelectList(await GetListApi<DealerGroupModelView>(TypeId: model.TypeId, Page: 1, PageSize: 20), "Id", "Name", model.DealerGroupId);
        }

        public override async Task<DealerModelView> InitializeData(DealerModelView ob)
        {
            ViewBag.DealersGroupList = new SelectList(await GetListApi<DealerGroupModelView>(TypeId: ob.TypeId , Page: 1, PageSize: 20), "Id", "Name", ob.DealerGroupId);
            if (ob == null)
                ob = new DealerModelView();
            if (ob.Id == 0)
            {
                ob.CodeNumber = long.Parse("0" + await GetValueApi<DealerModelView>($"GetMax?TypeId={ob.TypeId}")) + 1;
                ob.Code = "" + ob.CodeNumber;
            }
            ob.DealerGroupName = (await GetObApi<DealerGroupModelView>($"GetById?Id={ob.DealerGroupId ?? 0}"))?.Name;

            return ob;
        }

        public async Task<JsonResult> GetList(string txtSearch = "", long TypeId = 0, int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();
            long TypeDealerId = TypeId == 1 || TypeId == 3 ? 1 : 2;

            var itemsList = await GetListApi<DealerModelView>(TypeId: TypeDealerId , TextSearch: txtSearch, Page: page, PageSize: pageSize);
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