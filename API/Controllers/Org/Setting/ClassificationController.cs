using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Classification.Commands;
using Application.Commands.Org.Setting.Classification.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class ClassificationController(ISender sender) : BaseController<GetByIdClassificationQuery, SearchClassificationQuery , GetListClassificationQuery, CreateClassificationCommand, UpdateClassificationCommand, DeleteClassificationCommand, DeleteListClassificationCommand, ClassificationModelView>(sender)
    {

    }
}