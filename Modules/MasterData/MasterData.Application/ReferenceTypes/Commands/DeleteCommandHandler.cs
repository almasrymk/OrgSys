namespace MasterData.Application.ReferenceTypes.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteReferenceTypeCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.ReferenceType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteReferenceTypeCommand, MasterData.Domain.ReferenceType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.ReferenceType, bool>> CreateFilter(DeleteReferenceTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
