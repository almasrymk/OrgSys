namespace Catalog.Application.PriceLists.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdatePriceListCommand : PriceListDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.PriceList> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdatePriceListCommand, Catalog.Domain.PriceList>(_UnitOfWork, _Repository, mapper, _provider)
    {

    }
}
