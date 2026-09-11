namespace MasterData.Application.ReferenceTypes.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListReferenceTypeCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.ReferenceType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListReferenceTypeCommand, MasterData.Domain.ReferenceType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.ReferenceType, bool>> CreateFilter(DeleteListReferenceTypeCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
