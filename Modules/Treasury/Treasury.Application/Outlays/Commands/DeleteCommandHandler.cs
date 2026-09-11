namespace Treasury.Application.Outlays.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteOutlayCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.Outlay> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteOutlayCommand, Treasury.Domain.Outlay>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Treasury.Domain.Outlay, bool>> CreateFilter(DeleteOutlayCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}