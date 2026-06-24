namespace Application.Commands.Org.Accounts.Account.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxAccountQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Domain.Entities.Account> _Repository) : GetMaxCommandHandler<GetMaxAccountQuery, Domain.Entities.Account>(_Repository)
    {
        public override Expression<Func<Domain.Entities.Account, bool>> CreateFilter(GetMaxAccountQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<Domain.Entities.Account, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}