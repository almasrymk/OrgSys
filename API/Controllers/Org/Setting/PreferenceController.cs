using MasterData.Application.PaymentTypes.Queries;
using Administration.Application.Preferences.Commands;
using Administration.Application.Preferences.Queries;
using OrgSys.SharedKernel;
using Administration.Application;
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