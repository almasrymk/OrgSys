namespace Application.Commands.Org.Setting.Bank.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateBankCommand : BankDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Bank> _Repository, IMapper mapper) : CreateCommandHandler<CreateBankCommand, Domain.Entities.Bank>(_UnitOfWork, _Repository, mapper)
    {

    }
}