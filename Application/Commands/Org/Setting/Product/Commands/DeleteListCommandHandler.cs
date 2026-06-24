namespace Application.Commands.Org.Setting.Product.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using System.Linq.Expressions;

    public sealed record DeleteListProductCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Product> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListProductCommand, Domain.Entities.Product>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Domain.Entities.Product, bool>> CreateFilter(DeleteListProductCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteListProductCommand request)
        {
            #region UpdateProduct           
            var res = await RemoveDetails<ProductUnit>(e => request.Ids.Contains(e.ProductId));
            if (!res) return false;
            #endregion

            return res;
        }
    }
}