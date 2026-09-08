namespace Application.Commands.Org.Setting.ReferenceType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record CreateReferenceTypeCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.ReferenceType> _Repository , IMapper mapper) : CreateCommandHandler<CreateReferenceTypeCommand, Domain.Entities.ReferenceType>(_UnitOfWork, _Repository , mapper)
    {

    }
}
