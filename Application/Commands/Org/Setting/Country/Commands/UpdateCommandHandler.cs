namespace Application.Commands.Org.Setting.Country.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateCountryCommand(long Id , long? CountryId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Country> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateCountryCommand, Entity.Model.Country>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}