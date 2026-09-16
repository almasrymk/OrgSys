namespace SaaS.Application.Features.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteFeatureCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Feature> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteFeatureCommand, Feature>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Feature, bool>> CreateFilter(DeleteFeatureCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }

    public sealed record DeleteListFeatureCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Feature> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListFeatureCommand, Feature>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Feature, bool>> CreateFilter(DeleteListFeatureCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
