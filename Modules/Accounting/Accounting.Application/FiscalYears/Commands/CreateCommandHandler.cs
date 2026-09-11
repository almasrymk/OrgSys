namespace Accounting.Application.FiscalYears.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateFiscalYearCommand : FiscalYearDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.FiscalYear> _Repository, IMapper mapper) : CreateCommandHandler<CreateFiscalYearCommand, Accounting.Domain.FiscalYear>(_UnitOfWork, _Repository, mapper)
    {

    }
}
