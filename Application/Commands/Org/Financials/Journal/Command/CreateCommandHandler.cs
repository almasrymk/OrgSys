namespace Application.Commands.Org.Financials.Journal.Commands;

using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;

public sealed class CreateJournalCommand : Application.DTOs.JournalDto, ICommand , ICreateCommand<Result>;

public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Journal> _Repository, IMapper mapper) : CreateCommandHandler<CreateJournalCommand, Domain.Entities.Journal>(_UnitOfWork, _Repository , mapper)
{
     
}