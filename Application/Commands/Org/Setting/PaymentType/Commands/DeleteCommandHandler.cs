namespace Application.Commands.Org.Setting.PaymentType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeletePaymentTypeCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.PaymentType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePaymentTypeCommand, Entity.Model.PaymentType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.PaymentType, bool>> CreateFilter(DeletePaymentTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}