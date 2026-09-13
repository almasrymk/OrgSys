namespace CommercialDocuments.Application.InvoiceTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListInvoiceTypeCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<CommercialDocuments.Domain.InvoiceType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListInvoiceTypeCommand, CommercialDocuments.Domain.InvoiceType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<CommercialDocuments.Domain.InvoiceType, bool>> CreateFilter(DeleteListInvoiceTypeCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}