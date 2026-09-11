namespace Treasury.Application.FinancialTypes.Command;

using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;

public sealed class CreateFinancialTypeCommand : FinancialTypeDto, ICommand , ICreateCommand<Result>;

public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, 
    IRepository<Treasury.Domain.FinancialType> _Repository, 
    IMapper mapper) : CreateCommandHandler<CreateFinancialTypeCommand, Treasury.Domain.FinancialType>(_UnitOfWork, _Repository , mapper)
{
   
}