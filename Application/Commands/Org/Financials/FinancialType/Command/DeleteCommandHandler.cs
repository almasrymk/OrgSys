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

    public sealed record DeleteFinancialTypeCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<FinancialType> _Repository,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteFinancialTypeCommand, FinancialType>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Entity.Model.FinancialType, bool>> CreateFilter(DeleteFinancialTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }

    }
}