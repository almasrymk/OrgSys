namespace Application.Commands.Org.City.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using AutoMapper;
    using Domain.Abstraction;
    using System.Linq.Expressions;
    using Utility;

    public sealed record DeleteListCommand(List<long> Ids) : ICommand;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.City> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteListCommand, Entity.Model.City>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.City, bool>> CreateFilter(DeleteListCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Status.Deleted && e.Hide != true;
        }
    }
}