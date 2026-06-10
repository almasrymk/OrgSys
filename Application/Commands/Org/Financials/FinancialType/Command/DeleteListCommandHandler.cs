namespace Application.Commands.Org.Financials.FinancialType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.Model;
    using Entity.ModelView;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;

    public sealed record DeleteListFinancialTypeCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.FinancialType> _Repository, IServiceProvider _provider) :
        DeleteCommandHandler<DeleteListFinancialTypeCommand, Entity.Model.FinancialType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.FinancialType, bool>> CreateFilter(DeleteListFinancialTypeCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }

    }
}