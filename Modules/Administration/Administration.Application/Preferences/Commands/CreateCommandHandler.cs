namespace Administration.Application.Preferences.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    
    

    public sealed class CreatePreferenceCommand : Administration.Application.PreferenceDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.Preference> _Repository , IMapper mapper) : CreateCommandHandler<CreatePreferenceCommand, Administration.Domain.Preference>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}