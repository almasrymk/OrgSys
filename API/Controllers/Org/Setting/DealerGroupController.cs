using Parties.Application.DealerGroups.Queries;
using Parties.Application.DealerGroups.Commands;
using Parties.Application.DealerGroups.Queries;
using OrgSys.SharedKernel;
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