namespace Application.Commands.Org.Setting.Shift.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteShiftCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Shift> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteShiftCommand, Domain.Entities.Shift>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Shift, bool>> CreateFilter(DeleteShiftCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}