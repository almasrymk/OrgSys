namespace Application.Commands.Org.Setting.Preference.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class CreatePreferenceCommand : Entity.ModelView.PreferenceModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Preference> _Repository , IMapper mapper) : CreateCommandHandler<CreatePreferenceCommand, Entity.Model.Preference>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}