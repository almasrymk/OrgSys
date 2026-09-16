namespace Administration.Application.Users.Commands
{
    using OrgSys.SharedKernel;
    using Administration.Application.Security;
    using AutoMapper;
    using System.Net;

    public sealed class UpdateUserCommand : UserDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Administration.Domain.User> _Repository , IMapper mapper, IServiceProvider _provider, IPasswordHasher passwordHasher) : UpdateCommandHandler<UpdateUserCommand, Administration.Domain.User>(_UnitOfWork, _Repository , mapper , _provider)
    {
        public override async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existing = await _Repository.GetByFilterAsync(u => u.Id == request.Id, string.Empty);
            if (existing is null)
                return new Result(HttpStatusCode.NotFound, new List<Error> { new Error("User not found") });

            UserPasswordApplier.Apply(request, passwordHasher, existing.Password);
            return await base.Handle(request, cancellationToken);
        }
    }
}
