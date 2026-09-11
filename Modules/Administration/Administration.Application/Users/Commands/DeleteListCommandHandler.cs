namespace Administration.Application.Users.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListUserCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.User> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListUserCommand, Administration.Domain.User>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Administration.Domain.User, bool>> CreateFilter(DeleteListUserCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}