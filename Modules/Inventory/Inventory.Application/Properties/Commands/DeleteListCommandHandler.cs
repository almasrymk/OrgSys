namespace Inventory.Application.Properties.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListPropertyCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Property> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListPropertyCommand, Inventory.Domain.Property>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Inventory.Domain.Property, bool>> CreateFilter(DeleteListPropertyCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}