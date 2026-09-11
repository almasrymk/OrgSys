namespace MasterData.Application.PaymentTypes.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListPaymentTypeCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.PaymentType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListPaymentTypeCommand, MasterData.Domain.PaymentType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.PaymentType, bool>> CreateFilter(DeleteListPaymentTypeCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
