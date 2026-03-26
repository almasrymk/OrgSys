namespace Application.Commands.Org.Setting.District.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdateDistrictCommand : DistrictModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.District> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateDistrictCommand, Entity.Model.District>(_UnitOfWork, _Repository, mapper, _provider)
    {

    }
}