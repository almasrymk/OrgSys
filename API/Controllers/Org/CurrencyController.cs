using Application.Commands.Org.Setting.Currency.Commands;
using Application.Commands.Org.Setting.Currency.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Currency
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController(ISender sender) : BaseController<GetByIdCurrencyQuery, SearchCurrencyQuery, CreateCurrencyCommand, UpdateCurrencyCommand, DeleteCurrencyCommand, DeleteListCurrencyCommand, CurrencyModelView>(sender)
    {

    }
}
