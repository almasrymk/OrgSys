namespace Application.Commands.Org.Setting.FinancialType.Queries
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

    public sealed record GetMaxFinancialTypeQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<FinancialType> _Repository) : GetMaxCommandHandler<GetMaxFinancialTypeQuery, FinancialType>(_Repository)
    {
        public override Expression<Func<FinancialType, bool>> CreateFilter(GetMaxFinancialTypeQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<FinancialType, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}