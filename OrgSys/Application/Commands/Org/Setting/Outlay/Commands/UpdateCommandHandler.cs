namespace Application.Commands.Org.Setting.Outlay.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateOutlayCommand(long Id , long? OutlayId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Outlay> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateOutlayCommand, Entity.Model.Outlay>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}