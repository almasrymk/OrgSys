using Application.Commands.Org.DealerGroups.DealerGroup.Queries;
using Application.Commands.Org.Invoices.Invoice.Queries;
using Application.Commands.Org.Setting.DealerGroup.Commands;
using Application.Commands.Org.Setting.DealerGroup.Queries;
using Application.Commands.Org.Setting.Preference.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class DealerGroupController(ISender sender) : BaseController<GetByIdDealerGroupQuery, SearchDealerGroupQuery , GetListDealerGroupQuery, CreateDealerGroupCommand, UpdateDealerGroupCommand, DeleteDealerGroupCommand, DeleteListDealerGroupCommand, GetMaxDealerGroupQuery, DealerGroupModelView>(sender)
    {

    }
}