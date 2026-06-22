namespace Application.Commands.Org.Setting.InvoiceType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteInvoiceTypeCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.InvoiceType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteInvoiceTypeCommand, Domain.Entities.InvoiceType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.InvoiceType, bool>> CreateFilter(DeleteInvoiceTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}