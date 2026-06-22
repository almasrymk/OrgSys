namespace Application.Commands.Org.Setting.Dealer.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateDealerCommand : DealerModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Dealer> _Repository , IMapper mapper) : CreateCommandHandler<CreateDealerCommand, Domain.Entities.Dealer>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}