namespace Application.Commands.Org.Setting.DealerGroup.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdateDealerGroupCommand : DealerGroupModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.DealerGroup> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateDealerGroupCommand, Entity.Model.DealerGroup>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}