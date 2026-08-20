namespace Application.Commands.Org.Setting.FiscalYear.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateFiscalYearCommand : FiscalYearDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.FiscalYear> _Repository, IMapper mapper) : CreateCommandHandler<CreateFiscalYearCommand, Domain.Entities.FiscalYear>(_UnitOfWork, _Repository, mapper)
    {

    }
}
