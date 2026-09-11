namespace Application.Commands.Org.Setting.Preference.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Domain.Abstraction;
    using Application.DTOs;

    public sealed class CreatePreferenceCommand : Application.DTOs.PreferenceDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Preference> _Repository , IMapper mapper) : CreateCommandHandler<CreatePreferenceCommand, Domain.Entities.Preference>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}