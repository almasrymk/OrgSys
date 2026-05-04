namespace Application.Commands.Org.Setting.Preference.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeletePreferenceCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Preference> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePreferenceCommand, Entity.Model.Preference>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Preference, bool>> CreateFilter(DeletePreferenceCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}