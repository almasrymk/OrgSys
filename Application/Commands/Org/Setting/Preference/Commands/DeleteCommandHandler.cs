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

    public sealed record DeletePreferenceCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Preference> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePreferenceCommand, Domain.Entities.Preference>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Preference, bool>> CreateFilter(DeletePreferenceCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}