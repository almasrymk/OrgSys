namespace Application.Commands.Org.Setting.Role.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateRoleCommand(long Id , long? RoleId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Role> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateRoleCommand, Entity.Model.Role>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}