namespace Application.Commands.Org.Setting.Currency.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record CreateCurrencyCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Currency> _Repository , IMapper mapper) : CreateCommandHandler<CreateCurrencyCommand, Entity.Model.Currency>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}