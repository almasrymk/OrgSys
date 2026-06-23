namespace Application.Commands.Org.Financials.FinancialType.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Setting.Product.Commands;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using Application.DTOs;

    public sealed class UpdateFinancialTypeCommand : Application.DTOs.FinancialTypeDto, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Domain.Entities.FinancialType> _Repository ,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateFinancialTypeCommand, Domain.Entities.FinancialType>(_UnitOfWork, _Repository , mapper , _provider)
    {
    }
}