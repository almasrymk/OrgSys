namespace Application.Commands.Org.Financials.FinancialType.Commands;

using Application.Abstraction.Command;
using Application.Common.Commands;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Entity.ModelView;

public sealed class CreateFinancialTypeCommand : Entity.ModelView.FinancialTypeModelView, ICommand , ICreateCommand<Result>;

public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, 
    IRepository<Entity.Model.FinancialType> _Repository, 
    IMapper mapper) : CreateCommandHandler<CreateFinancialTypeCommand, Entity.Model.FinancialType>(_UnitOfWork, _Repository , mapper)
{
   
}