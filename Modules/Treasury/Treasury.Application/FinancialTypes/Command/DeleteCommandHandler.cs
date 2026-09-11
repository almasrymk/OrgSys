namespace Treasury.Application.FinancialTypes.Command
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;

    public sealed record DeleteFinancialTypeCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<FinancialType> _Repository,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteFinancialTypeCommand, FinancialType>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<FinancialType, bool>> CreateFilter(DeleteFinancialTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

    }
}