namespace Organization.Application.Shifts.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdateShiftCommand(long Id , long? ShiftId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Shift> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateShiftCommand, Organization.Domain.Shift>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}