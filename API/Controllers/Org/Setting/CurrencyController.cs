using Application.Commands.Org.Setting.Country.Queries;
using Application.Commands.Org.Setting.Currency.Commands;
using Application.Commands.Org.Setting.Currency.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
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