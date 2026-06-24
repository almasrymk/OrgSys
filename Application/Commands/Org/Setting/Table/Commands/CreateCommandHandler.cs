namespace Application.Commands.Org.Setting.Table.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed record CreateTableCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Table> _Repository , IMapper mapper) : CreateCommandHandler<CreateTableCommand, Domain.Entities.Table>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}