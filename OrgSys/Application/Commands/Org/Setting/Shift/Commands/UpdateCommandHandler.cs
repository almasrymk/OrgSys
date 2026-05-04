namespace Application.Commands.Org.Setting.Shift.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateShiftCommand(long Id , long? ShiftId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Shift> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateShiftCommand, Entity.Model.Shift>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}