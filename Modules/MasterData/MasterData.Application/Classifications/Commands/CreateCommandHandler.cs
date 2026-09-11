namespace MasterData.Application.Classifications.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateClassificationCommand: ClassificationDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.Classification> _Repository , IMapper mapper) : CreateCommandHandler<CreateClassificationCommand, MasterData.Domain.Classification>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}
