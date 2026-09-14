namespace Administration.Application.Preferences.Queries;

using Administration.Contracts.Preferences;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetPreferenceValueQueryHandler(IRepository<Preference> _Repository) : IQueryHandler<GetPreferenceValueQuery, string?>
{
    public async Task<Result<string?>> Handle(GetPreferenceValueQuery request, CancellationToken cancellationToken)
    {
        var preferences = (await _Repository.GetListByFilterAsync(
            e => e.Reference == request.Reference && e.TypeId == request.TypeId))?.ToList() ?? [];

        var value = preferences.FirstOrDefault(e => e.Key == request.Key)?.Value;
        return new Result<string?>(HttpStatusCode.OK, string.IsNullOrWhiteSpace(value) ? null : value, null);
    }
}
