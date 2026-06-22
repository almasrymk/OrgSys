namespace Application.Commands.Org.Setting.Bank.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class UpdateBankCommand : BankModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Bank> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateBankCommand, Domain.Entities.Bank>(_UnitOfWork, _Repository, mapper , _provider)
    {

    }
}