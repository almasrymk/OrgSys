namespace Application.Commands.Org.Setting.Currency.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class UpdateCurrencyCommand : CurrencyModelView , ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Currency> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateCurrencyCommand, Domain.Entities.Currency>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}