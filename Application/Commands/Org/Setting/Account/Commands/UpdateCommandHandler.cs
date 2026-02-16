namespace Application.Commands.Org.Setting.Account.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdateAccountCommand : Entity.ModelView.AccountModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Account> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateAccountCommand, Entity.Model.Account>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}