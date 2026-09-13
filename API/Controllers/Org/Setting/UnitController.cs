using MasterData.Application.Units.Commands;
using MasterData.Application.Units.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class UnitController(ISender sender) : BaseController<GetByIdUnitQuery, SearchUnitQuery , GetListUnitQuery, CreateUnitCommand, UpdateUnitCommand, DeleteUnitCommand, DeleteListUnitCommand, UnitDto>(sender)
    {

    }
}