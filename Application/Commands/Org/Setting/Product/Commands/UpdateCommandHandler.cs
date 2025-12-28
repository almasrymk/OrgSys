namespace Application.Commands.Org.Setting.Product.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed record UpdateProductCommand(long Id , long? ProductId, string Name) : ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Product> _Repository , IMapper mapper) : UpdateCommandHandler<UpdateProductCommand, Entity.Model.Product>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}