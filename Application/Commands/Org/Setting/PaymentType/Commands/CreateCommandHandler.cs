namespace Application.Commands.Org.Setting.PaymentType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreatePaymentTypeCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.PaymentType> _Repository , IMapper mapper) : CreateCommandHandler<CreatePaymentTypeCommand, Entity.Model.PaymentType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}