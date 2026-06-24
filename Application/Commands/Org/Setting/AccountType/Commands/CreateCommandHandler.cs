namespace Application.Commands.Org.Setting.AccountType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record CreateAccountTypeCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.AccountType> _Repository , IMapper mapper) : CreateCommandHandler<CreateAccountTypeCommand, Domain.Entities.AccountType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}