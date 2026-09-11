namespace Administration.Application.Users.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateUserCommand : UserDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.User> _Repository , IMapper mapper) : CreateCommandHandler<CreateUserCommand, Administration.Domain.User>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}