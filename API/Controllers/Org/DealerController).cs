using Application.Commands.Org.Setting.Currency.Queries;
using Application.Commands.Org.Setting.Dealer.Commands;
using Application.Commands.Org.Setting.Dealer.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Dealer
{
    [ApiController]
    [Route("[controller]")]
    public class DealerController(ISender sender) : BaseController<GetByIdDealerQuery, SearchDealerQuery , GetListDealerQuery, CreateDealerCommand, UpdateDealerCommand, DeleteDealerCommand, DeleteListDealerCommand, DealerModelView>(sender)
    {

    }
}