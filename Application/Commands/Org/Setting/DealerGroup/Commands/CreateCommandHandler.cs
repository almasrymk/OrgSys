namespace Application.Commands.Org.Setting.DealerGroup.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateDealerGroupCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.DealerGroup> _Repository , IMapper mapper) : CreateCommandHandler<CreateDealerGroupCommand, Entity.Model.DealerGroup>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}