namespace Application.Commands.Org.Dealers.Dealer.Queries
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

    public sealed record GetMaxDealerQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Dealer> _Repository) : GetMaxCommandHandler<GetMaxDealerQuery, Dealer>(_Repository)
    {
        public override Expression<Func<Dealer, bool>> CreateFilter(GetMaxDealerQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<Dealer, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}