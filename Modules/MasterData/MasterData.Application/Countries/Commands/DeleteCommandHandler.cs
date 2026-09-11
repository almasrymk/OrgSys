namespace MasterData.Application.Countries.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteCountryCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Country> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteCountryCommand, MasterData.Domain.Country>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.Country, bool>> CreateFilter(DeleteCountryCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
