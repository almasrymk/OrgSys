namespace Catalog.Application.PriceLists.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreatePriceListCommand : PriceListDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.PriceList> _Repository, IMapper mapper) : CreateCommandHandler<CreatePriceListCommand, Catalog.Domain.PriceList>(_UnitOfWork, _Repository, mapper)
    {

    }
}
