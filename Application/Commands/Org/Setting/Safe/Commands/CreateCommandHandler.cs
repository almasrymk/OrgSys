namespace Application.Commands.Org.Setting.Safe.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateSafeCommand : SafeModelView , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Safe> _Repository , IMapper mapper) : CreateCommandHandler<CreateSafeCommand, Domain.Entities.Safe>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}