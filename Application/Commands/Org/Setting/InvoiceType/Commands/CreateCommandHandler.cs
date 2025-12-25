namespace Application.Commands.Org.Setting.InvoiceType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateInvoiceTypeCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.InvoiceType> _Repository , IMapper mapper) : CreateCommandHandler<CreateInvoiceTypeCommand, Entity.Model.InvoiceType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}