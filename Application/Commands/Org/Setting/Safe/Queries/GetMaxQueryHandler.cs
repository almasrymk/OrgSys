namespace Application.Commands.Org.Accounts.Safe.Queries
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

    public sealed record GetMaxSafeQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Domain.Entities.Safe> _Repository) : GetMaxCommandHandler<GetMaxSafeQuery, Domain.Entities.Safe>(_Repository)
    {
        public override Expression<Func<Safe, bool>> CreateFilter(GetMaxSafeQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<Safe, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}