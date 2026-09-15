namespace Catalog.Application.Attributes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListPropertyCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Property> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListPropertyCommand, Catalog.Domain.Property>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Catalog.Domain.Property, bool>> CreateFilter(DeleteListPropertyCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}