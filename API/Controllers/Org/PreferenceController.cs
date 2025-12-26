using Application.Commands.Org.Setting.PaymentType.Queries;
using Application.Commands.Org.Setting.Preference.Commands;
using Application.Commands.Org.Setting.Preference.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Preference
{
    [ApiController]
    [Route("[controller]")]
    public class PreferenceController(ISender sender) : BaseController<GetByIdPreferenceQuery, SearchPreferenceQuery , GetListPreferenceQuery, CreatePreferenceCommand, UpdatePreferenceCommand, DeletePreferenceCommand, DeleteListPreferenceCommand, PreferenceModelView>(sender)
    {

    }
}