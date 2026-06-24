namespace Application.Commands.Org.Setting.Unit.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateUnitCommand: UnitDto , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Unit> _Repository , IMapper mapper) : CreateCommandHandler<CreateUnitCommand, Domain.Entities.Unit>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}