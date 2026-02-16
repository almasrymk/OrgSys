namespace Application.Commands.Org.Setting.AccountType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateAccountTypeCommand(long Id , long? AccountTypeId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.AccountType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateAccountTypeCommand, Entity.Model.AccountType>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}