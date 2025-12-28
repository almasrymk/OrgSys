namespace Application.Commands.Org.Setting.Account.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateAccountCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Account> _Repository , IMapper mapper) : CreateCommandHandler<CreateAccountCommand, Entity.Model.Account>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}