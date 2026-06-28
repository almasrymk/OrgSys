using Application.Abstraction.Query;
using AutoMapper;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Commands.Org.Auth.Queries
{

    public sealed record CheckPasswordQuery(string email, string passwoed) : IQuery<bool>;


    public sealed class CheckPasswordQueryHandler(IRepository<User> Repository) : IRequestHandler<CheckPasswordQuery, Result<bool>>
    {
        public async Task<Result<bool>> Handle(CheckPasswordQuery request, CancellationToken cancellationToken)
        {
            var password = Utility.Security.Encrypt(request.passwoed);
            password = "fTxWMjHA5MbUktJph2vqIlc9Gu1cU5MrbdYztkd5yec=";

         bool exist = await   Repository.AnyAsync(u => u.UserName == request.email && u.Password == password);

           return  new Result<bool>( exist == true ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                             exist , exist == true ? null : new List<Error> { new Error("Error") });
        }
    }
}
