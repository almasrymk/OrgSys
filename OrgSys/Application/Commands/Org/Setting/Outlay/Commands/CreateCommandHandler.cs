namespace Application.Commands.Org.Setting.Outlay.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateOutlayCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Outlay> _Repository , IMapper mapper) : CreateCommandHandler<CreateOutlayCommand, Entity.Model.Outlay>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}