namespace Catalog.Application.Categories.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateClassificationCommand: ClassificationDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Classification> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateClassificationCommand, Catalog.Domain.Classification>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}
