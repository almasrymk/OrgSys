namespace Application.Commands.Org.Transactions.TransactionType.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.Model;
    using Entity.ModelView;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxTransactionTypeQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Entity.Model.TransactionType> _Repository) : GetMaxCommandHandler<GetMaxTransactionTypeQuery, Entity.Model.TransactionType>(_Repository)
    {
        public override Expression<Func<TransactionType, object>> CreateSelector()
        {
            return e => e.Id;
        }
    }
}