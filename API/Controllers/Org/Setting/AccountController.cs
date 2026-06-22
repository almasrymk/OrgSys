using Application.Commands.Org.Accounts.Account.Queries;
using Application.Commands.Org.Dealers.Dealer.Queries;
using Application.Commands.Org.Setting.Account.Commands;
using Application.Commands.Org.Setting.Account.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController(ISender sender) : BaseController<GetByIdAccountQuery, SearchAccountQuery , GetListAccountQuery, CreateAccountCommand, UpdateAccountCommand, DeleteAccountCommand, DeleteListAccountCommand , GetMaxAccountQuery, AccountModelView>(sender)
    {

    }
}