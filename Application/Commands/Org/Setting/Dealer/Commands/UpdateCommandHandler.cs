namespace Application.Commands.Org.Setting.Dealer.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class UpdateDealerCommand : DealerModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Dealer> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateDealerCommand, Domain.Entities.Dealer>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}