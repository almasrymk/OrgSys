namespace Application.Commands.Org.Setting.Account.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateAccountCommand(long Id , long? AccountId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Account> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateAccountCommand, Entity.Model.Account>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}