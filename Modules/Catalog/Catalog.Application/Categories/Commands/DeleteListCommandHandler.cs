namespace Catalog.Application.Categories.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListClassificationCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Classification> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListClassificationCommand, Catalog.Domain.Classification>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Catalog.Domain.Classification, bool>> CreateFilter(DeleteListClassificationCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
