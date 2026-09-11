namespace Sales.Application.InvoiceTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteInvoiceTypeCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Sales.Domain.InvoiceType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteInvoiceTypeCommand, Sales.Domain.InvoiceType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Sales.Domain.InvoiceType, bool>> CreateFilter(DeleteInvoiceTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}