namespace Inventory.Application.Properties.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeletePropertyCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Property> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePropertyCommand, Inventory.Domain.Property>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Inventory.Domain.Property, bool>> CreateFilter(DeletePropertyCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}