namespace Application.Commands.Org.Setting.Shift.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record UpdateShiftCommand(long Id , long? ShiftId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Shift> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateShiftCommand, Domain.Entities.Shift>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}