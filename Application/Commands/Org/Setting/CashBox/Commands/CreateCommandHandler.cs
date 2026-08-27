namespace Application.Commands.Org.Setting.CashBox.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateCashBoxCommand : CashBoxDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.CashBox> _Repository, IMapper mapper) : CreateCommandHandler<CreateCashBoxCommand, Domain.Entities.CashBox>(_UnitOfWork, _Repository, mapper)
    {

    }
}
