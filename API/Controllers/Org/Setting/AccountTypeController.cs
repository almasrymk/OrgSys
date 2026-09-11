using MediatR;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Accounting.Application.AccountTypes.Queries;
using Accounting.Application.AccountTypes.Commands;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class AccountTypeController(ISender sender) : BaseController<GetByIdAccountTypeQuery, SearchAccountTypeQuery , GetListAccountTypeQuery, CreateAccountTypeCommand, UpdateAccountTypeCommand, DeleteAccountTypeCommand, DeleteListAccountTypeCommand, AccountTypeDto>(sender)
    {

    }
}