using Application.Commands.Org.Setting.Preference.Queries;
using Organization.Application.Shifts.Commands;
using Organization.Application.Shifts.Queries;
using OrgSys.SharedKernel;
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