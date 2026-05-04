using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Outlay.Commands;
using Application.Commands.Org.Setting.Outlay.Queries;
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
    public class OutlayController(ISender sender) : BaseController<GetByIdOutlayQuery, SearchOutlayQuery , GetListOutlayQuery, CreateOutlayCommand, UpdateOutlayCommand, DeleteOutlayCommand, DeleteListOutlayCommand, OutlayModelView>(sender)
    {

    }
}