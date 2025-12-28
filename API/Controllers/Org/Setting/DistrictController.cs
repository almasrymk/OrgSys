using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.District.Commands;
using Application.Commands.Org.Setting.District.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class DistrictController(ISender sender) : BaseController<GetByIdDistrictQuery, SearchDistrictQuery , GetListDistrictQuery, CreateDistrictCommand, UpdateDistrictCommand, DeleteDistrictCommand, DeleteListDistrictCommand, DistrictModelView>(sender)
    {

    }
}