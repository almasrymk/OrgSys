namespace Parties.Application.DealerGroups.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateDealerGroupCommand: DealerGroupDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Parties.Domain.DealerGroup> _Repository , IMapper mapper) : CreateCommandHandler<CreateDealerGroupCommand, Parties.Domain.DealerGroup>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}