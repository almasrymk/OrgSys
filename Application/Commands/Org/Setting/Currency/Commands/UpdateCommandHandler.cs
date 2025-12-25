namespace Application.Commands.Org.Setting.Currency.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateCurrencyCommand(long Id , long? CurrencyId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Currency> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateCurrencyCommand, Entity.Model.Currency>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}