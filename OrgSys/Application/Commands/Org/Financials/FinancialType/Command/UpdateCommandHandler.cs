namespace Application.Commands.Org.Financials.FinancialType.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Setting.Product.Commands;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.Model;
    using Entity.ModelView;

    public sealed class UpdateFinancialTypeCommand : Entity.ModelView.FinancialTypeModelView, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Entity.Model.FinancialType> _Repository ,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateFinancialTypeCommand, Entity.Model.FinancialType>(_UnitOfWork, _Repository , mapper , _provider)
    {
    }
}