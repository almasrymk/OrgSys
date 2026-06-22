namespace Application.Commands.Org.Setting.Branch.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record UpdateBranchCommand(long Id , string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Branch> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateBranchCommand, Domain.Entities.Branch>(_UnitOfWork, _Repository , mapper, _provider)
    {
       
    }
}