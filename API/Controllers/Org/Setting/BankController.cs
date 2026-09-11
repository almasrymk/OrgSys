using Application.Commands.Org.Setting.Preference.Queries;
using Treasury.Application.Banks.Commands;
using Treasury.Application.Banks.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class BankController(ISender sender) : BaseController<GetByIdBankQuery, SearchBankQuery , GetListBankQuery, CreateBankCommand, UpdateBankCommand, DeleteBankCommand, DeleteListBankCommand, BankDto>(sender)
    {

    }
}