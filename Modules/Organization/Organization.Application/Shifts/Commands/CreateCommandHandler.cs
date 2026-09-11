namespace Organization.Application.Shifts.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreateShiftCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Shift> _Repository , IMapper mapper) : CreateCommandHandler<CreateShiftCommand, Organization.Domain.Shift>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}