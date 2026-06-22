namespace Application.Commands.Org.Financials.Financial.Commands
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

    public sealed record GetMaxFinancialQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Financial> _Repository) : GetMaxCommandHandler<GetMaxFinancialQuery, Financial>(_Repository)
    {
        public override Expression<Func<Financial, bool>> CreateFilter(GetMaxFinancialQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<Financial, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}