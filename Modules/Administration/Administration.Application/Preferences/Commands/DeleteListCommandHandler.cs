namespace Administration.Application.Preferences.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    
    
    using System.Linq.Expressions;

    public sealed record DeleteListPreferenceCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.Preference> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListPreferenceCommand, Administration.Domain.Preference>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Administration.Domain.Preference, bool>> CreateFilter(DeleteListPreferenceCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}