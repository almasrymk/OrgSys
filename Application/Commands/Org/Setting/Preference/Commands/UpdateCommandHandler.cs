namespace Application.Commands.Org.Setting.Preference.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdatePreferenceCommand : Entity.ModelView.PreferenceModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Preference> _Repository , IMapper mapper) : UpdateCommandHandler<UpdatePreferenceCommand, Entity.Model.Preference>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}