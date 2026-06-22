namespace Application.Commands.Org.Financials.FinancialType.Commands;

using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Application.DTOs;

public sealed class CreateFinancialTypeCommand : Application.DTOs.FinancialTypeModelView, ICommand , ICreateCommand<Result>;

public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, 
    IRepository<Domain.Entities.FinancialType> _Repository, 
    IMapper mapper) : CreateCommandHandler<CreateFinancialTypeCommand, Domain.Entities.FinancialType>(_UnitOfWork, _Repository , mapper)
{
   
}