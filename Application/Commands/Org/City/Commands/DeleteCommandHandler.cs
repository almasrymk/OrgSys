namespace Application.Commands.Org.City.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.City.Queries;
    using Application.Common.Commands;
    using AutoMapper;
    using Domain.Abstraction;
    using System.Linq.Expressions;
    using Utility;

    public sealed record DeleteCommand(long Id) : ICommand;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.City> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteCommand, Entity.Model.City>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.City, bool>> CreateFilter(DeleteCommand request)
        {
            return e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true;
        }
    }
}