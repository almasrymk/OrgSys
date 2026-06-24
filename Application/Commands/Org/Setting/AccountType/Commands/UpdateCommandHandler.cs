namespace Application.Commands.Org.Setting.AccountType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record UpdateAccountTypeCommand(long Id , long? AccountTypeId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.AccountType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateAccountTypeCommand, Domain.Entities.AccountType>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}