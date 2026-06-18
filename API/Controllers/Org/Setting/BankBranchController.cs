using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.BankBranch.Commands;
using Application.Commands.Org.Setting.BankBranch.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class BankBranchController(ISender sender) : BaseController<GetByIdBankBranchQuery, SearchBankBranchQuery , GetListBankBranchQuery, CreateBankBranchCommand, UpdateBankBranchCommand, DeleteBankBranchCommand, DeleteListBankBranchCommand, BankBranchModelView>(sender)
    {

    }
}