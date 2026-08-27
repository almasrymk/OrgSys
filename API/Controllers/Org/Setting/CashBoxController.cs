using Application.Commands.Org.Setting.CashBox.Commands;
using Application.Commands.Org.Setting.CashBox.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using Application.DTOs;
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
