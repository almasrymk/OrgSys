namespace Treasury.Application.Outlays.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListOutlayCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.Outlay> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListOutlayCommand, Treasury.Domain.Outlay>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Treasury.Domain.Outlay, bool>> CreateFilter(DeleteListOutlayCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}