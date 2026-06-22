namespace Application.Commands.Org.Setting.District.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteDistrictCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.District> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteDistrictCommand, Domain.Entities.District>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.District, bool>> CreateFilter(DeleteDistrictCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}