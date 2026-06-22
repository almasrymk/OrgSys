namespace Application.Commands.Org.Setting.Classification.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class UpdateClassificationCommand: ClassificationModelView, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Classification> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateClassificationCommand, Domain.Entities.Classification>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}