namespace Application.Commands.Org.Setting.Shift.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateShiftCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Shift> _Repository , IMapper mapper) : CreateCommandHandler<CreateShiftCommand, Entity.Model.Shift>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}