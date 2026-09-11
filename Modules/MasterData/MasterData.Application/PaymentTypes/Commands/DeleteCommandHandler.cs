namespace MasterData.Application.PaymentTypes.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeletePaymentTypeCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.PaymentType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePaymentTypeCommand, MasterData.Domain.PaymentType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.PaymentType, bool>> CreateFilter(DeletePaymentTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
