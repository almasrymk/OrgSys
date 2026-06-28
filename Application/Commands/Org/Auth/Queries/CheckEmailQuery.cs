using Application.Abstraction.Command;
using Application.Abstraction.Query;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using MediatR;
using System.Collections;
using System.Net;


namespace Application.Commands.Org.Auth.Queries
{
    public sealed record CheckEmailQuery(string Email) : IQuery<bool>;


    public sealed class CheckEmailQueryHandler(IRepository<Domain.Entities.User> _Repository) : IRequestHandler<CheckEmailQuery, Result<bool>>
    {
        public async Task<Result<bool>> Handle(CheckEmailQuery request, CancellationToken cancellationToken)
        {
            var exists = await _Repository.AnyAsync(
                x => x.UserName == request.Email,
                cancellationToken);


            return new Result<bool>(exists == true ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                              exists, exists == true ? null : new List<Error> { new Error("Error") });
        }
    }
}
