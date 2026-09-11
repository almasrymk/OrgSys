namespace Treasury.Application.FinancialTypes.Command
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;

    public sealed record DeleteListFinancialTypeCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<FinancialType> _Repository, IServiceProvider _provider) :
        DeleteCommandHandler<DeleteListFinancialTypeCommand, FinancialType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<FinancialType, bool>> CreateFilter(DeleteListFinancialTypeCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

    }
}