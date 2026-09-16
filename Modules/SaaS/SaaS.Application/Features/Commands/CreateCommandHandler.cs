namespace SaaS.Application.Features.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateFeatureCommand : FeatureDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Feature> _Repository, IMapper mapper) : CreateCommandHandler<CreateFeatureCommand, Feature>(_UnitOfWork, _Repository, mapper)
    {
    }
}
