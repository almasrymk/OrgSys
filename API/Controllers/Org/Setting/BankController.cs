using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Bank.Commands;
using Application.Commands.Org.Setting.Bank.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class BankController(ISender sender) : BaseController<GetByIdBankQuery, SearchBankQuery , GetListBankQuery, CreateBankCommand, UpdateBankCommand, DeleteBankCommand, DeleteListBankCommand, BankModelView>(sender)
    {

    }
}