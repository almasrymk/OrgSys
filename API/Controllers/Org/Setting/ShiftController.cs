using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Shift.Commands;
using Application.Commands.Org.Setting.Shift.Queries;
using Application.Interfaces.CQRS;
using Domain.Shared;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class ShiftController(ISender sender) : BaseController<GetByIdShiftQuery, SearchShiftQuery , GetListShiftQuery, CreateShiftCommand, UpdateShiftCommand, DeleteShiftCommand, DeleteListShiftCommand, ShiftDto>(sender)
    {

    }
}