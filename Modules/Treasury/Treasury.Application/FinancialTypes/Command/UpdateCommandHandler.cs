namespace Treasury.Application.FinancialTypes.Command
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdateFinancialTypeCommand : FinancialTypeDto, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<FinancialType> _Repository ,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateFinancialTypeCommand, FinancialType>(_UnitOfWork, _Repository , mapper , _provider)
    {
    }
}