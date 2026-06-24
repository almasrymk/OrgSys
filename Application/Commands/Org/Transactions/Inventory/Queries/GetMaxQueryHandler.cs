namespace Application.Commands.Org.Transactions.Inventory.Queries
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

    public sealed record GetMaxInventoryQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Inventory> _Repository) : GetMaxCommandHandler<GetMaxInventoryQuery, Inventory>(_Repository)
    {
        public override Expression<Func<Inventory, bool>> CreateFilter(GetMaxInventoryQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<Inventory, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}