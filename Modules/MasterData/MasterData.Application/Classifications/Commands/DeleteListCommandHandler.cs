namespace MasterData.Application.Classifications.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListClassificationCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Classification> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListClassificationCommand, MasterData.Domain.Classification>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.Classification, bool>> CreateFilter(DeleteListClassificationCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
