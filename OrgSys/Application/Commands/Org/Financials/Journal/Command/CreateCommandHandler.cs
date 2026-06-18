namespace Application.Commands.Org.Financials.Journal.Commands;

using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;

public sealed class CreateJournalCommand : Entity.ModelView.JournalModelView, ICommand , ICreateCommand<Result>;

public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Journal> _Repository, IMapper mapper) : CreateCommandHandler<CreateJournalCommand, Entity.Model.Journal>(_UnitOfWork, _Repository , mapper)
{
     
}