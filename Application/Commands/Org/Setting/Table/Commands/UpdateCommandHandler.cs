namespace Application.Commands.Org.Setting.Table.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record UpdateTableCommand(long Id , long? TableId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Table> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateTableCommand, Domain.Entities.Table>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}