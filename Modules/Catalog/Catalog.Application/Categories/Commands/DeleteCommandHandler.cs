namespace Catalog.Application.Categories.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteClassificationCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Classification> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteClassificationCommand, Catalog.Domain.Classification>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Catalog.Domain.Classification, bool>> CreateFilter(DeleteClassificationCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
