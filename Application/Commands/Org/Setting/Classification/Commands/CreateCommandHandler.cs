namespace Application.Commands.Org.Setting.Classification.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateClassificationCommand: ClassificationModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Classification> _Repository , IMapper mapper) : CreateCommandHandler<CreateClassificationCommand, Domain.Entities.Classification>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}