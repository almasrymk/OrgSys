namespace OrgSys.Areas.Setting.Controllers
{
    using MasterData.Application.Classifications.Commands;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class ClassificationController(IConfiguration configuration, IMapper mapper) : MainController<ClassificationDto, CreateClassificationCommand, UpdateClassificationCommand>(configuration, mapper)
    {
        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<ClassificationDto>(TextSearch: txtSearch, Page: page, PageSize: pageSize);
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