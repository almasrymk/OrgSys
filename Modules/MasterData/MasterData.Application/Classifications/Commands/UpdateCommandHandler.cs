namespace MasterData.Application.Classifications.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateClassificationCommand: ClassificationDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Classification> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateClassificationCommand, MasterData.Domain.Classification>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}
