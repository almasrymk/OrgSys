namespace Accounting.Application.FiscalYears.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class UpdateFiscalYearCommand : FiscalYearDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.FiscalYear> _Repository, IRepository<Accounting.Domain.FiscalPeriod> _FiscalPeriodRepository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateFiscalYearCommand, Accounting.Domain.FiscalYear>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<bool> SaveDetials(UpdateFiscalYearCommand request)
        {
            var periods = request.Periods ?? [];
            var ids = periods.Where(e => e.Id > 0).Select(e => e.Id).ToList();
            var removeList = await _FiscalPeriodRepository.GetListByFilterAsync(e => e.FiscalYearId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<Accounting.Domain.FiscalPeriod>(removeList ?? []);
            if (!res) return false;

            var ob = mapper.Map<List<Accounting.Domain.FiscalPeriod>>(periods);
            foreach (var period in ob)
                period.FiscalYearId = request.Id;

            return await UpdateDetails<Accounting.Domain.FiscalPeriod>(ob);
        }
    }
}
