namespace Application.Commands.Org.Setting.Preference.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Domain.Abstraction;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeletePreferenceCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Preference> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePreferenceCommand, Domain.Entities.Preference>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Preference, bool>> CreateFilter(DeletePreferenceCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}