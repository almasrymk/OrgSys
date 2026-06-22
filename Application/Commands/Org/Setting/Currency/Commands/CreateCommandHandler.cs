namespace Application.Commands.Org.Setting.Currency.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateCurrencyCommand : CurrencyModelView , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Currency> _Repository , IMapper mapper) : CreateCommandHandler<CreateCurrencyCommand, Domain.Entities.Currency>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}