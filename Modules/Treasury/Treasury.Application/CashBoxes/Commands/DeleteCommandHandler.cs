namespace Treasury.Application.CashBoxes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteCashBoxCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.CashBox> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteCashBoxCommand, Treasury.Domain.CashBox>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Treasury.Domain.CashBox, bool>> CreateFilter(DeleteCashBoxCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
