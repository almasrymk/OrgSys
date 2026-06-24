namespace Application.Commands.Org.Setting.District.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class UpdateDistrictCommand : DistrictDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.District> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateDistrictCommand, Domain.Entities.District>(_UnitOfWork, _Repository, mapper, _provider)
    {

    }
}