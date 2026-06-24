namespace Application.Commands.Org.Setting.Unit.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class UpdateUnitCommand : UnitDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Unit> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateUnitCommand, Domain.Entities.Unit>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}