namespace Application.Commands.Org.Setting.Preference.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreatePreferenceCommand : Application.DTOs.PreferenceModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Preference> _Repository , IMapper mapper) : CreateCommandHandler<CreatePreferenceCommand, Domain.Entities.Preference>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}