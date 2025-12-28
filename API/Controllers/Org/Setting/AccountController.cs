using MediatR;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Application.Commands.Org.Setting.Account.Queries;
using Application.Commands.Org.Setting.Account.Commands;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController(ISender sender) : BaseController<GetByIdAccountQuery, SearchAccountQuery , GetListAccountQuery, CreateAccountCommand, UpdateAccountCommand, DeleteAccountCommand, DeleteListAccountCommand, AccountModelView>(sender)
    {

    }
}