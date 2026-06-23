namespace Application.Commands.Org.Setting.Classification.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetByIdClassificationQuery(long Id) : ICommand<ClassificationDto> , IGetByIdQuery<Result<ClassificationDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Classification> _Repository, IMapper mapper) : GetCommandHandler<GetByIdClassificationQuery, Domain.Entities.Classification, ClassificationDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Classification, bool>> CreateFilter(GetByIdClassificationQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}