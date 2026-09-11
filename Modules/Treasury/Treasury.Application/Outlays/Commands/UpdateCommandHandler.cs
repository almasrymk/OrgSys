namespace Treasury.Application.Outlays.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record UpdateOutlayCommand(long Id , long? OutlayId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.Outlay> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateOutlayCommand, Treasury.Domain.Outlay>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}