namespace Application.Commands.Org.Setting.InvoiceType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record UpdateInvoiceTypeCommand(long Id , long? InvoiceTypeId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.InvoiceType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateInvoiceTypeCommand, Domain.Entities.InvoiceType>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}