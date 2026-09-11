using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;
using MediatR;
using System.Collections;
using System.Net;


namespace Administration.Application.Auth.Queries
{
    public sealed record CheckEmailQuery(string Email) : IQuery<bool>;


    public sealed class CheckEmailQueryHandler(IRepository<Administration.Domain.User> _Repository) : IRequestHandler<CheckEmailQuery, Result<bool>>
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
