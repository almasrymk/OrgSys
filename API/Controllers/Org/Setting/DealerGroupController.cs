using Sales.Application.DealerGroups.Queries;
using Sales.Application.Invoices.Queries;
using Sales.Application.DealerGroups.Commands;
using Sales.Application.DealerGroups.Queries;
using Application.Commands.Org.Setting.Preference.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class DealerGroupController(ISender sender) : BaseController<GetByIdDealerGroupQuery, SearchDealerGroupQuery , GetListDealerGroupQuery, CreateDealerGroupCommand, UpdateDealerGroupCommand, DeleteDealerGroupCommand, DeleteListDealerGroupCommand, GetMaxDealerGroupQuery, DealerGroupDto>(sender)
    {

    }
}