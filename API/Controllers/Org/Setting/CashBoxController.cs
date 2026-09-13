using Treasury.Application.CashBoxes.Commands;
using Treasury.Application.CashBoxes.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class CashBoxController(ISender sender) : BaseController<GetByIdCashBoxQuery, SearchCashBoxQuery, GetListCashBoxQuery, CreateCashBoxCommand, UpdateCashBoxCommand, DeleteCashBoxCommand, DeleteListCashBoxCommand, GetMaxCashBoxQuery, CashBoxDto>(sender)
    {

    }
}
