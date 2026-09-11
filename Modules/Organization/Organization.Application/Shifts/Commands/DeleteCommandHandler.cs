namespace Organization.Application.Shifts.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteShiftCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Shift> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteShiftCommand, Organization.Domain.Shift>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Organization.Domain.Shift, bool>> CreateFilter(DeleteShiftCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}