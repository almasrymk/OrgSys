namespace Application.Commands.Org.Setting.Account.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateAccountCommand : Application.DTOs.AccountModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Account> _Repository , IMapper mapper) : CreateCommandHandler<CreateAccountCommand, Domain.Entities.Account>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}