using Application.Commands.Org.Setting.Preference.Queries;
using Organization.Application.Branches.Commands;
using Organization.Application.Branches.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class BranchController(ISender sender) : BaseController<GetByIdBranchQuery, SearchBranchQuery , GetListBranchQuery, CreateBranchCommand, UpdateBranchCommand, DeleteBranchCommand, DeleteListBranchCommand, BranchDto>(sender)
    {

    }
}