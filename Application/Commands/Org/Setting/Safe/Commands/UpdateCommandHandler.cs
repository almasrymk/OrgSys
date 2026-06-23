namespace Application.Commands.Org.Setting.Safe.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class UpdateSafeCommand : SafeDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Safe> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateSafeCommand, Domain.Entities.Safe>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}