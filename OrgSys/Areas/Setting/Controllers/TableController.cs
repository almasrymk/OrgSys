namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Table.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;

    [Area("Setting")]
    public class TableController(IConfiguration configuration, IMapper mapper) : MainController<TableModelView, CreateTableCommand, UpdateTableCommand>(configuration, mapper)
    {

    }
}