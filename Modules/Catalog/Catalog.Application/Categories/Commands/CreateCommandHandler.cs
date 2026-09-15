namespace Catalog.Application.Categories.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateClassificationCommand: ClassificationDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Classification> _Repository , IMapper mapper) : CreateCommandHandler<CreateClassificationCommand, Catalog.Domain.Classification>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
