namespace Application.Commands.Org.Setting.District.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateDistrictCommand: DistrictModelView , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.District> _Repository , IMapper mapper) : CreateCommandHandler<CreateDistrictCommand, Domain.Entities.District>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}