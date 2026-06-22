using Application.Commands.Org.Dealers.Dealer.Queries;
using Application.Commands.Org.Setting.Dealer.Commands;
using Application.Commands.Org.Setting.Dealer.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class DealerController(ISender sender) : BaseController<GetByIdDealerQuery, SearchDealerQuery , GetListDealerQuery, CreateDealerCommand, UpdateDealerCommand, DeleteDealerCommand, DeleteListDealerCommand , GetMaxDealerQuery, DealerModelView>(sender)
    {

    }
}