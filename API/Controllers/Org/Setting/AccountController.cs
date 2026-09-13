using Parties.Application.Dealers.Queries;
using Accounting.Application.Accounts.Commands;
using Accounting.Application.Accounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController(ISender sender) : BaseController<GetByIdAccountQuery, SearchAccountQuery , GetListAccountQuery, CreateAccountCommand, UpdateAccountCommand, DeleteAccountCommand, DeleteListAccountCommand , GetMaxAccountQuery, AccountDto>(sender)
    {

    }
}