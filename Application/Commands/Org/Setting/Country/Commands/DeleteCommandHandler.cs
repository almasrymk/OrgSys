namespace Application.Commands.Org.Setting.Country.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteCountryCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Country> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteCountryCommand, Domain.Entities.Country>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Country, bool>> CreateFilter(DeleteCountryCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}