using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Branch.Commands;
using Application.Commands.Org.Setting.Branch.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class BranchController(ISender sender) : BaseController<GetByIdBranchQuery, SearchBranchQuery , GetListBranchQuery, CreateBranchCommand, UpdateBranchCommand, DeleteBranchCommand, DeleteListBranchCommand, BranchModelView>(sender)
    {

    }
}