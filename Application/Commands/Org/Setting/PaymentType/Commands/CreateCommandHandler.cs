namespace Application.Commands.Org.Setting.PaymentType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record CreatePaymentTypeCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.PaymentType> _Repository , IMapper mapper) : CreateCommandHandler<CreatePaymentTypeCommand, Domain.Entities.PaymentType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}