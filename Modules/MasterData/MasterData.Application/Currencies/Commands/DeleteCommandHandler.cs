namespace MasterData.Application.Currencies.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteCurrencyCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Currency> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteCurrencyCommand, MasterData.Domain.Currency>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.Currency, bool>> CreateFilter(DeleteCurrencyCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
