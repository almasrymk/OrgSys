namespace Treasury.Application.Banks.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListBankCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.Bank> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListBankCommand, Treasury.Domain.Bank>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Treasury.Domain.Bank, bool>> CreateFilter(DeleteListBankCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}