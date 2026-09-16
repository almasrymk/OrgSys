namespace SaaS.Application.Features.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateFeatureCommand : FeatureDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Feature> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateFeatureCommand, Feature>(_UnitOfWork, _Repository, mapper, _provider)
    {
    }
}
