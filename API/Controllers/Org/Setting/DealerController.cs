using Sales.Application.Dealers.Queries;
using Sales.Application.Dealers.Commands;
using Sales.Application.Dealers.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class DealerController(ISender sender) : BaseController<GetByIdDealerQuery, SearchDealerQuery , GetListDealerQuery, CreateDealerCommand, UpdateDealerCommand, DeleteDealerCommand, DeleteListDealerCommand , GetMaxDealerQuery, DealerDto>(sender)
    {
        [HttpGet("Balance")]
        public Task<Result<decimal>> Balance(long Id, DateTime? AsOfDate, CancellationToken cancellationToken)
        {
            return Sender.Send(new GetDealerBalanceQuery(Id, AsOfDate), cancellationToken);
        }
    }
}