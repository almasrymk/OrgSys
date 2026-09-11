namespace Sales.Application.InvoiceTypes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdateInvoiceTypeCommand(long Id , long? InvoiceTypeId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Sales.Domain.InvoiceType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateInvoiceTypeCommand, Sales.Domain.InvoiceType>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}