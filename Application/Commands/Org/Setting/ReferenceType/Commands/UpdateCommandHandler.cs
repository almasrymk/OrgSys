namespace Application.Commands.Org.Setting.ReferenceType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record UpdateReferenceTypeCommand(long Id, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.ReferenceType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateReferenceTypeCommand, Domain.Entities.ReferenceType>(_UnitOfWork, _Repository , mapper , _provider)
    {

    }
}
