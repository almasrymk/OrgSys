namespace Application.Commands.Org.Country.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListCountryCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Country> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteListCountryCommand, Entity.Model.Country>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Country, bool>> CreateFilter(DeleteListCountryCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}