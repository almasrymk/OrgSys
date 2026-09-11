namespace Administration.Application.Users.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteUserCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.User> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteUserCommand, Administration.Domain.User>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Administration.Domain.User, bool>> CreateFilter(DeleteUserCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}