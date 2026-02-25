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

    public sealed record DeleteListPreferenceCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Preference> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListPreferenceCommand, Entity.Model.Preference>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Preference, bool>> CreateFilter(DeleteListPreferenceCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}