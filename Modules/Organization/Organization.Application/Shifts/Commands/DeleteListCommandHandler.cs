namespace Organization.Application.Shifts.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListShiftCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Shift> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListShiftCommand, Organization.Domain.Shift>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Organization.Domain.Shift, bool>> CreateFilter(DeleteListShiftCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}