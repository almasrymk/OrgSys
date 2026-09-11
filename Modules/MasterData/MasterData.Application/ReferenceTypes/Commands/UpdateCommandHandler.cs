namespace MasterData.Application.ReferenceTypes.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdateReferenceTypeCommand(long Id, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.ReferenceType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateReferenceTypeCommand, MasterData.Domain.ReferenceType>(_UnitOfWork, _Repository , mapper , _provider)
    {

    }
}
