namespace MasterData.Application.Classifications.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteClassificationCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Classification> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteClassificationCommand, MasterData.Domain.Classification>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.Classification, bool>> CreateFilter(DeleteClassificationCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
