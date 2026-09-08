using Application.Commands.Org.Setting.ReferenceType.Commands;
using Application.Commands.Org.Setting.ReferenceType.Queries;
using Application.Interfaces.CQRS;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class ReferenceTypeController(ISender sender) : BaseController<GetByIdReferenceTypeQuery, SearchReferenceTypeQuery , GetListReferenceTypeQuery, CreateReferenceTypeCommand, UpdateReferenceTypeCommand, DeleteReferenceTypeCommand, DeleteListReferenceTypeCommand, ReferenceTypeDto>(sender)
    {

    }
}
