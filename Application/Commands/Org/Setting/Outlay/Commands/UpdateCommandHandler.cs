namespace Application.Commands.Org.Setting.Outlay.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record UpdateOutlayCommand(long Id , long? OutlayId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Outlay> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateOutlayCommand, Domain.Entities.Outlay>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}