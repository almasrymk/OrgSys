namespace MasterData.Application.Currencies.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListCurrencyCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Currency> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListCurrencyCommand, MasterData.Domain.Currency>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.Currency, bool>> CreateFilter(DeleteListCurrencyCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
