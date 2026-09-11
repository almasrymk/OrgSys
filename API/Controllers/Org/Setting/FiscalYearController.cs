using Accounting.Application.FiscalYears.Commands;
using Accounting.Application.FiscalYears.Queries;
using OrgSys.SharedKernel;
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
