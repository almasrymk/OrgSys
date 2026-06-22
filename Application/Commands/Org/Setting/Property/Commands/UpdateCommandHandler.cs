namespace Application.Commands.Org.Setting.Property.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record UpdatePropertyCommand(long Id , long? PropertyId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Property> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdatePropertyCommand, Domain.Entities.Property>(_UnitOfWork, _Repository , mapper,_provider)
    {
       
    }
}