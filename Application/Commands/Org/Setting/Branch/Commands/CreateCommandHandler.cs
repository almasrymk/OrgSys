namespace Application.Commands.Org.Setting.Branch.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;

    public sealed record CreateBranchCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Branch> _Repository , IMapper mapper) : CreateCommandHandler<CreateBranchCommand, Domain.Entities.Branch>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}