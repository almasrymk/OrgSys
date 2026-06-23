namespace Application.Commands.Org.Setting.BankBranch.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateBankBranchCommand : BankBranchDto , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.BankBranch> _Repository , IMapper mapper) : CreateCommandHandler<CreateBankBranchCommand, Domain.Entities.BankBranch>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}