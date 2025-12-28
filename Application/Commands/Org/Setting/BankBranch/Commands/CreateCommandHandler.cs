namespace Application.Commands.Org.Setting.BankBranch.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateBankBranchCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.BankBranch> _Repository , IMapper mapper) : CreateCommandHandler<CreateBankBranchCommand, Entity.Model.BankBranch>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}