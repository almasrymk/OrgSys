namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Outlay.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;

    [Area("Setting")]
    public class OutlayController(IConfiguration configuration, IMapper mapper) : MainController<OutlayModelView, CreateOutlayCommand, UpdateOutlayCommand>(configuration, mapper)
    {

    }
}