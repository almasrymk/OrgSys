namespace Application.Commands.Org.Setting.FinancialType.Command
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using Application.DTOs;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;

    public sealed record DeleteListFinancialTypeCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<FinancialType> _Repository, IServiceProvider _provider) :
        DeleteCommandHandler<DeleteListFinancialTypeCommand, FinancialType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<FinancialType, bool>> CreateFilter(DeleteListFinancialTypeCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

    }
}