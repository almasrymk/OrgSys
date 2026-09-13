namespace Administration.Application.Preferences.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    
    
    using System.Linq.Expressions;

    public sealed record DeletePreferenceCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.Preference> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePreferenceCommand, Administration.Domain.Preference>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Administration.Domain.Preference, bool>> CreateFilter(DeletePreferenceCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}