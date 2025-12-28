using MediatR;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Application.Commands.Org.Setting.AccountType.Queries;
using Application.Commands.Org.Setting.AccountType.Commands;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class AccountTypeController(ISender sender) : BaseController<GetByIdAccountTypeQuery, SearchAccountTypeQuery , GetListAccountTypeQuery, CreateAccountTypeCommand, UpdateAccountTypeCommand, DeleteAccountTypeCommand, DeleteListAccountTypeCommand, AccountTypeModelView>(sender)
    {

    }
}