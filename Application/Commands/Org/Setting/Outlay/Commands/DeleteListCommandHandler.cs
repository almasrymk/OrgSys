namespace Application.Commands.Org.Setting.Outlay.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteListOutlayCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Outlay> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListOutlayCommand, Domain.Entities.Outlay>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Outlay, bool>> CreateFilter(DeleteListOutlayCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}