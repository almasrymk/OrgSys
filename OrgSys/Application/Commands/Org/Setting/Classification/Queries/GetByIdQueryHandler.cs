namespace Application.Commands.Org.Setting.Classification.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record GetByIdClassificationQuery(long Id) : ICommand<ClassificationModelView> , IGetByIdQuery<Result<ClassificationModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Classification> _Repository, IMapper mapper) : GetCommandHandler<GetByIdClassificationQuery, Entity.Model.Classification, ClassificationModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Classification, bool>> CreateFilter(GetByIdClassificationQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}