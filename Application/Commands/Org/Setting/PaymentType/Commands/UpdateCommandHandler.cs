namespace Application.Commands.Org.Setting.PaymentType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record UpdatePaymentTypeCommand(long Id , long? PaymentTypeId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.PaymentType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdatePaymentTypeCommand, Domain.Entities.PaymentType>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}