namespace OrgSys.Areas.Setting.Controllers
{
    using Accounting.Application.FiscalYears.Commands;
    using AutoMapper;
    using Application.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class FiscalYearController(IConfiguration configuration, IMapper mapper) : MainController<FiscalYearDto, CreateFiscalYearCommand, UpdateFiscalYearCommand>(configuration, mapper)
    {
        public override async Task<FiscalYearDto> InitializeData(FiscalYearDto ob)
        {
            if (ob.Id == 0)
            {
                var today = DateTime.Today;
                ob.StartDate = new DateTime(today.Year, 1, 1);
                ob.EndDate = new DateTime(today.Year, 12, 31);
            }

            ob.Periods ??= [];

            return ob;
        }
    }
}
