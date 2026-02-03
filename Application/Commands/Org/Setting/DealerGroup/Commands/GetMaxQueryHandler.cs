namespace Application.Commands.Org.DealerGroups.DealerGroup.Queries
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

    public sealed record GetMaxDealerGroupQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Entity.Model.DealerGroup> _Repository) : GetMaxCommandHandler<GetMaxDealerGroupQuery, Entity.Model.DealerGroup>(_Repository)
    {
        public override Expression<Func<DealerGroup, bool>> CreateFilter(GetMaxDealerGroupQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<DealerGroup, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}