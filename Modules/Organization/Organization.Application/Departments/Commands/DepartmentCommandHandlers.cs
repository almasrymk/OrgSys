namespace Organization.Application.Departments.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreateDepartmentCommand : DepartmentDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Department> _Repository, IMapper mapper)
        : CreateCommandHandler<CreateDepartmentCommand, Organization.Domain.Department>(_UnitOfWork, _Repository, mapper)
    {
    }

    public sealed class UpdateDepartmentCommand : DepartmentDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Department> _Repository, IMapper mapper, IServiceProvider _provider)
        : UpdateCommandHandler<UpdateDepartmentCommand, Organization.Domain.Department>(_UnitOfWork, _Repository, mapper, _provider)
    {
    }

    public sealed record DeleteDepartmentCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Department> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeleteDepartmentCommand, Organization.Domain.Department>(_UnitOfWork, _Repository, _provider)
    {
        public override System.Linq.Expressions.Expression<Func<Organization.Domain.Department, bool>> CreateFilter(DeleteDepartmentCommand request) =>
            e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true;
    }

    public sealed record DeleteListDepartmentCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Organization.Domain.Department> _Repository, IServiceProvider _provider)
        : DeleteCommandHandler<DeleteListDepartmentCommand, Organization.Domain.Department>(_UnitOfWork, _Repository, _provider)
    {
        public override System.Linq.Expressions.Expression<Func<Organization.Domain.Department, bool>> CreateFilter(DeleteListDepartmentCommand request) =>
            e => request.Ids.Contains(e.Id) && e.Status != Status.Deleted && e.Hide != true;
    }
}
