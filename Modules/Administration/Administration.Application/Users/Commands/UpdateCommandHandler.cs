namespace Administration.Application.Users.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateUserCommand : UserDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.User> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateUserCommand, Administration.Domain.User>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}