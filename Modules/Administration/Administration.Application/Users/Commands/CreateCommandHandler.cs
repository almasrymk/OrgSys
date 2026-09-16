namespace Administration.Application.Users.Commands
{
    using OrgSys.SharedKernel;
    using Administration.Application.Security;
    using AutoMapper;

    public sealed class CreateUserCommand : UserDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.User> _Repository , IMapper mapper, IPasswordHasher passwordHasher) : CreateCommandHandler<CreateUserCommand, Administration.Domain.User>(_UnitOfWork, _Repository , mapper)
    {
        public override Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            UserPasswordApplier.Apply(request, passwordHasher);
            return base.Handle(request, cancellationToken);
        }
    }
}
