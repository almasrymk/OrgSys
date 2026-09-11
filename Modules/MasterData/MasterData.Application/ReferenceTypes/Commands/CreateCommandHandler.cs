namespace MasterData.Application.ReferenceTypes.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreateReferenceTypeCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<MasterData.Domain.ReferenceType> _Repository , IMapper mapper) : CreateCommandHandler<CreateReferenceTypeCommand, MasterData.Domain.ReferenceType>(_UnitOfWork, _Repository , mapper)
    {

    }
}
