namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Dealer.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using Service;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class DealerController(IConfiguration configuration, IMapper mapper) : MainController<DealerModelView, CreateDealerCommand, UpdateDealerCommand>(configuration, mapper)
    {

        public override async Task LoadViewBag(DealerModelView model)
        {
            ViewBag.DealersGroupList = new SelectList(new DealerGroupService(User.GetSchema()).GetAll(model.ParentId, model.TypeId), "Id", "Name", model.DealerGroupId);
        }

        public override async Task<DealerModelView> InitializeData(DealerModelView ob)
        {
            ViewBag.DealersGroupList = new SelectList(new DealerGroupService(User.GetSchema()).GetAll(ob.ParentId, ob.TypeId), "Id", "Name", ob.DealerGroupId);
            if (ob == null)
                ob = new DealerModelView();
            if (ob.Id == 0)
            {
                ob.CodeNumber = new DealerService(User.GetSchema()).GetMaxCode(ob.TypeId);
                ob.Code = "" + ob.CodeNumber;
            }
            return ob;
        }

        public JsonResult GetList(string txtSearch = "", long TypeId = 0, int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();
            long TypeDealerId = TypeId == 1 || TypeId == 3 ? 1 : 2;

            var itemsList = new DealerService(User.GetSchema()).GetAll(txtSearch, 0, TypeDealerId, page, pageSize);
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