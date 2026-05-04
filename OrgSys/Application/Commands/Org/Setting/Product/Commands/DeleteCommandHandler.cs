namespace Application.Commands.Org.Setting.Product.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.Model;
    using System.Linq.Expressions;

    public sealed record DeleteProductCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Product> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteProductCommand, Entity.Model.Product>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Product, bool>> CreateFilter(DeleteProductCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }

        override public async Task<bool> RemoveDetails(DeleteProductCommand request)
        {
            #region UpdateProduct           
            var res = await RemoveDetails<ProductUnit>(e=>e.ProductId == request.Id);
            if (!res) return false;           
            #endregion 

            return res;
        }
    }
}