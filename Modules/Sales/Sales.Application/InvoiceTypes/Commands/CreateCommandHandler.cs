namespace Sales.Application.InvoiceTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreateInvoiceTypeCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Sales.Domain.InvoiceType> _Repository , IMapper mapper) : CreateCommandHandler<CreateInvoiceTypeCommand, Sales.Domain.InvoiceType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}