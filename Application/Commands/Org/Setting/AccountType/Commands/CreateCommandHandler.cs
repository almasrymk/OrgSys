namespace Application.Commands.Org.Setting.AccountType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateAccountTypeCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.AccountType> _Repository , IMapper mapper) : CreateCommandHandler<CreateAccountTypeCommand, Entity.Model.AccountType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}