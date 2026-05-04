namespace Application.Commands.Org.Setting.BankBranch.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdateBankBranchCommand : BankBranchModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.BankBranch> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateBankBranchCommand, Entity.Model.BankBranch>(_UnitOfWork, _Repository, mapper, _provider)
    {

    }
}