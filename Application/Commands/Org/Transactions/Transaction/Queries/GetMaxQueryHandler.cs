namespace Application.Commands.Org.Transactions.Transaction.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using Application.DTOs;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxTransactionQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Domain.Entities.Transaction> _Repository) : GetMaxCommandHandler<GetMaxTransactionQuery, Domain.Entities.Transaction>(_Repository)
    {
        public override Expression<Func<Transaction, bool>> CreateFilter(GetMaxTransactionQuery request)
        {
            return e => e.TypeId == request.TypeId
                || ((request.TypeId == 1 || request.TypeId == 5) && (e.TypeId == 1 || e.TypeId == 5))
                || ((request.TypeId == 2 || request.TypeId == 6) && (e.TypeId == 2 || e.TypeId == 6));
        }

        public override Expression<Func<Transaction, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}
