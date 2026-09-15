namespace Catalog.Application.Brands.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateBrandCommand : BrandDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Brand> _Repository, IMapper mapper, IServiceProvider provider) : UpdateCommandHandler<UpdateBrandCommand, Catalog.Domain.Brand>(_UnitOfWork, _Repository, mapper, provider)
    {

    }
}
