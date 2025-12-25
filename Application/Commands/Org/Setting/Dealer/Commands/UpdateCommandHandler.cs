namespace Application.Commands.Org.Setting.Dealer.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateDealerCommand(long Id , long? DealerId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Dealer> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateDealerCommand, Entity.Model.Dealer>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}