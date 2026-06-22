namespace Application.Commands.Org.Setting.Product.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateProductCommand : ProductModelView , ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Product> _Repository , IMapper mapper) : CreateCommandHandler<CreateProductCommand, Domain.Entities.Product>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}