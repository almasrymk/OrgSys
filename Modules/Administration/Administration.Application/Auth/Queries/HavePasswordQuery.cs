using OrgSys.SharedKernel;
using AutoMapper;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Administration.Application.Auth.Queries
{
    public sealed record HavePasswordQuery(string Email) : IQuery<bool>;


    public sealed class CHavePasswordHandler(IRepository<Administration.Domain.User> _Repository) : IRequestHandler<HavePasswordQuery, Result<bool>>
    {
        public async Task<Result<bool>> Handle(HavePasswordQuery request, CancellationToken cancellationToken)
        {
            var exists = await _Repository.AnyAsync(
                x => x.UserName == request.Email && x.Password != "",
                cancellationToken);

            return new Result<bool>(exists == true ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                              exists, exists == true ? null : new List<Error> { new Error("Error") });
        }
    }
}
