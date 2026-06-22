namespace Application.Commands.Org.Setting.Preference.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteListPreferenceCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Preference> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListPreferenceCommand, Domain.Entities.Preference>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Preference, bool>> CreateFilter(DeleteListPreferenceCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}