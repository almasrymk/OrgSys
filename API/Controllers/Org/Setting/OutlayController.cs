using Treasury.Application.Outlays.Commands;
using Treasury.Application.Outlays.Queries;
using OrgSys.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class OutlayController(ISender sender) : BaseController<GetByIdOutlayQuery, SearchOutlayQuery , GetListOutlayQuery, CreateOutlayCommand, UpdateOutlayCommand, DeleteOutlayCommand, DeleteListOutlayCommand, OutlayDto>(sender)
    {

    }
}