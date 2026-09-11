namespace Sales.Application.DealerGroups.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateDealerGroupCommand : DealerGroupDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Sales.Domain.DealerGroup> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateDealerGroupCommand, Sales.Domain.DealerGroup>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}