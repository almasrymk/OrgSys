namespace Application.Commands.Org.Setting.Account.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class UpdateAccountCommand : Application.DTOs.AccountModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Account> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateAccountCommand, Domain.Entities.Account>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}