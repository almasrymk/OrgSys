namespace Catalog.Application.Brands.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateBrandCommand : BrandDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Brand> _Repository, IMapper mapper) : CreateCommandHandler<CreateBrandCommand, Catalog.Domain.Brand>(_UnitOfWork, _Repository, mapper)
    {

    }
}
