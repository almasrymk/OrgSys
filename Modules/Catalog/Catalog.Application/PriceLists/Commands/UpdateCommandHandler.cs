namespace Catalog.Application.PriceLists.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdatePriceListCommand : PriceListDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Catalog.Domain.PriceList> _Repository,
        IRepository<Catalog.Domain.PriceListEntry> entryRepository,
        IMapper mapper,
        IServiceProvider _provider)
        : UpdateCommandHandler<UpdatePriceListCommand, Catalog.Domain.PriceList>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<bool> SaveDetials(UpdatePriceListCommand request)
        {
            var entries = request.Entries ?? [];
            var ids = entries.Where(e => e.Id > 0).Select(e => e.Id).ToList();
            var removeList = await entryRepository.GetListByFilterAsync(e => e.PriceListId == request.Id && !ids.Contains(e.Id));

            var removed = await RemoveDetails<PriceListEntry>(removeList ?? []);
            if (!removed) return false;

            var mapped = mapper.Map<List<PriceListEntry>>(entries);
            foreach (var entry in mapped)
                entry.PriceListId = request.Id;

            return await UpdateDetails<PriceListEntry>(mapped);
        }
    }
}
