using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Unit.Commands;
using Application.Commands.Org.Setting.Unit.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class UnitController(ISender sender) : BaseController<GetByIdUnitQuery, SearchUnitQuery , GetListUnitQuery, CreateUnitCommand, UpdateUnitCommand, DeleteUnitCommand, DeleteListUnitCommand, UnitModelView>(sender)
    {

    }
}