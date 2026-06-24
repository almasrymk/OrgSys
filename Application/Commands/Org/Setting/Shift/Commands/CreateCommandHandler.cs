namespace Application.Commands.Org.Setting.Shift.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record CreateShiftCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Shift> _Repository , IMapper mapper) : CreateCommandHandler<CreateShiftCommand, Domain.Entities.Shift>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}