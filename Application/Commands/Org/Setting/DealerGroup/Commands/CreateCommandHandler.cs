namespace Application.Commands.Org.Setting.DealerGroup.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateDealerGroupCommand: DealerGroupModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.DealerGroup> _Repository , IMapper mapper) : CreateCommandHandler<CreateDealerGroupCommand, Domain.Entities.DealerGroup>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}