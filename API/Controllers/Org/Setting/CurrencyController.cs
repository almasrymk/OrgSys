using MasterData.Application.Countries.Queries;
using MasterData.Application.Currencies.Commands;
using MasterData.Application.Currencies.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController(ISender sender) : BaseController<GetByIdCurrencyQuery, SearchCurrencyQuery , GetListCurrencyQuery, CreateCurrencyCommand, UpdateCurrencyCommand, DeleteCurrencyCommand, DeleteListCurrencyCommand, CurrencyDto>(sender)
    {

    }
}