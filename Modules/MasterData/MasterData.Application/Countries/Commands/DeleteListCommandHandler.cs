namespace MasterData.Application.Countries.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListCountryCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Country> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListCountryCommand, MasterData.Domain.Country>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<MasterData.Domain.Country, bool>> CreateFilter(DeleteListCountryCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
