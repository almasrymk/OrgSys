using Treasury.Application.BankBranches.Commands;
using Treasury.Application.BankBranches.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class BankBranchController(ISender sender) : BaseController<GetByIdBankBranchQuery, SearchBankBranchQuery , GetListBankBranchQuery, CreateBankBranchCommand, UpdateBankBranchCommand, DeleteBankBranchCommand, DeleteListBankBranchCommand, BankBranchDto>(sender)
    {

    }
}