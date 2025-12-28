namespace Application.Commands.Org.Setting.District.Commands
{
    using Application.Abstraction.Command;    
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteDistrictCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.District> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteDistrictCommand, Entity.Model.District>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.District, bool>> CreateFilter(DeleteDistrictCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}