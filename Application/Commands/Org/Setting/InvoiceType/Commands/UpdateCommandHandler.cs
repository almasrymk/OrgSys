namespace Application.Commands.Org.Setting.InvoiceType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateInvoiceTypeCommand(long Id , long? InvoiceTypeId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.InvoiceType> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateInvoiceTypeCommand, Entity.Model.InvoiceType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}