namespace Application.Commands.Org.Setting.Bank.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdateBankCommand : BankModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Bank> _Repository, IMapper mapper) : UpdateCommandHandler<UpdateBankCommand, Entity.Model.Bank>(_UnitOfWork, _Repository, mapper)
    {

    }
}