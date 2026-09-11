namespace Treasury.Application.CashBoxes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListCashBoxCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.CashBox> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListCashBoxCommand, Treasury.Domain.CashBox>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Treasury.Domain.CashBox, bool>> CreateFilter(DeleteListCashBoxCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
