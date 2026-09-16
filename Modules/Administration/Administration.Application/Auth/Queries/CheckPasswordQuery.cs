using OrgSys.SharedKernel;
using Administration.Application.Security;
using MediatR;
using System.Net;

namespace Administration.Application.Auth.Queries
{

    public sealed record CheckPasswordQuery(string email, string passwoed) : IQuery<bool>;


    public sealed class CheckPasswordQueryHandler(IRepository<User> Repository, IPasswordHasher passwordHasher) : IRequestHandler<CheckPasswordQuery, Result<bool>>
    {
        public async Task<Result<bool>> Handle(CheckPasswordQuery request, CancellationToken cancellationToken)
        {
            var user = await Repository.GetByFilterAsync(u => u.UserName == request.email, string.Empty);

            bool exist = user is not null
                && !string.IsNullOrEmpty(user.Password)
                && !user.MustResetPassword
                && passwordHasher.VerifyHashedPassword(user.Password, request.passwoed);

           return  new Result<bool>( exist == true ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                             exist , exist == true ? null : new List<Error> { new Error("Error") });
        }
    }
}
