using Application.Commands.Org.Setting.FiscalYear.Commands;
using Application.Commands.Org.Setting.FiscalYear.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class FiscalYearController(ISender sender) : BaseController<GetByIdFiscalYearQuery, SearchFiscalYearQuery, GetListFiscalYearQuery, CreateFiscalYearCommand, UpdateFiscalYearCommand, DeleteFiscalYearCommand, DeleteListFiscalYearCommand, FiscalYearDto>(sender)
    {

    }
}
