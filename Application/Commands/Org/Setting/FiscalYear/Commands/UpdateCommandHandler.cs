namespace Application.Commands.Org.Setting.FiscalYear.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class UpdateFiscalYearCommand : FiscalYearDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.FiscalYear> _Repository, IRepository<Domain.Entities.FiscalPeriod> _FiscalPeriodRepository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateFiscalYearCommand, Domain.Entities.FiscalYear>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<bool> SaveDetials(UpdateFiscalYearCommand request)
        {
            var periods = request.Periods ?? [];
            var ids = periods.Where(e => e.Id > 0).Select(e => e.Id).ToList();
            var removeList = await _FiscalPeriodRepository.GetListByFilterAsync(e => e.FiscalYearId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<Domain.Entities.FiscalPeriod>(removeList ?? []);
            if (!res) return false;

            var ob = mapper.Map<List<Domain.Entities.FiscalPeriod>>(periods);
            foreach (var period in ob)
                period.FiscalYearId = request.Id;

            return await UpdateDetails<Domain.Entities.FiscalPeriod>(ob);
        }
    }
}
