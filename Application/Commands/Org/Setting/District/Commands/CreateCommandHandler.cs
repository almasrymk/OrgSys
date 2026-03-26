namespace Application.Commands.Org.Setting.District.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class CreateDistrictCommand: DistrictModelView , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.District> _Repository , IMapper mapper) : CreateCommandHandler<CreateDistrictCommand, Entity.Model.District>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}