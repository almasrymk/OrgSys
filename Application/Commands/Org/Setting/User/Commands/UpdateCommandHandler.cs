namespace Application.Commands.Org.Setting.User.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class UpdateUserCommand : UserDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.User> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateUserCommand, Domain.Entities.User>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}