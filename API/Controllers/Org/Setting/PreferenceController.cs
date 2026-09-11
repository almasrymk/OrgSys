using MasterData.Application.PaymentTypes.Queries;
using Application.Commands.Org.Setting.Preference.Commands;
using Application.Commands.Org.Setting.Preference.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class PreferenceController(ISender sender) : BaseController<GetByIdPreferenceQuery, SearchPreferenceQuery , GetListPreferenceQuery, CreatePreferenceCommand, UpdatePreferenceCommand, DeletePreferenceCommand, DeleteListPreferenceCommand, PreferenceDto>(sender)
    {

    }
}