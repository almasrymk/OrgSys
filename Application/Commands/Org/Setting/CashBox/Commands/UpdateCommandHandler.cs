namespace Application.Commands.Org.Setting.CashBox.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class UpdateCashBoxCommand : CashBoxDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.CashBox> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateCashBoxCommand, Domain.Entities.CashBox>(_UnitOfWork, _Repository, mapper, _provider)
    {

    }
}
