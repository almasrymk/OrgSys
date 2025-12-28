namespace Application.Commands.Org.Setting.Branch.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateBranchCommand(long Id , long? BranchId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Branch> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateBranchCommand, Entity.Model.Branch>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}