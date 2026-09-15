namespace Catalog.Application.Attributes.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed record CreatePropertyCommand(string Name) : ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Catalog.Domain.Property> _Repository , IMapper mapper) : CreateCommandHandler<CreatePropertyCommand, Catalog.Domain.Property>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}