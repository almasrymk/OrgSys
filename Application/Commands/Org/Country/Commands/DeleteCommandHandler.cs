namespace Application.Commands.Org.Country.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.City.Queries;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;
   
    public sealed record DeleteCountryCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Country> _Repository , IMapper mapper) : DeleteCommandHandler<DeleteCountryCommand, Entity.Model.Country>(_UnitOfWork, _Repository , mapper)
    {
        public override Expression<Func<Entity.Model.Country, bool>> CreateFilter(DeleteCountryCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}