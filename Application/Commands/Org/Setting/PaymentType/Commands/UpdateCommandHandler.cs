namespace Application.Commands.Org.Setting.PaymentType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdatePaymentTypeCommand(long Id , long? PaymentTypeId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.PaymentType> _Repository , IMapper mapper) : UpdateCommandHandler<UpdatePaymentTypeCommand, Entity.Model.PaymentType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}