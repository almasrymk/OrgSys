namespace CommercialDocuments.Application.InvoiceTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreateInvoiceTypeCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<CommercialDocuments.Domain.InvoiceType> _Repository , IMapper mapper) : CreateCommandHandler<CreateInvoiceTypeCommand, CommercialDocuments.Domain.InvoiceType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}