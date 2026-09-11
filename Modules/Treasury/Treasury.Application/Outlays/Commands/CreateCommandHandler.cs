namespace Treasury.Application.Outlays.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreateOutlayCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Treasury.Domain.Outlay> _Repository , IMapper mapper) : CreateCommandHandler<CreateOutlayCommand, Treasury.Domain.Outlay>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}