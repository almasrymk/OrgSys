namespace Administration.Application.Preferences.Queries;

using Administration.Contracts.Preferences;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetPreferenceValuesQueryHandler(IRepository<Preference> repository)
    : IQueryHandler<GetPreferenceValuesQuery, IReadOnlyDictionary<string, string?>>
{
    public async Task<Result<IReadOnlyDictionary<string, string?>>> Handle(
        GetPreferenceValuesQuery request,
        CancellationToken cancellationToken)
    {
        var preferences = (await repository.GetListByFilterAsync(
            e => e.Reference == request.Reference && e.TypeId == request.TypeId))?.ToList() ?? [];

        IReadOnlyDictionary<string, string?> values = preferences
            .Where(p => !string.IsNullOrWhiteSpace(p.Key))
            .GroupBy(p => p.Key!)
            .ToDictionary(g => g.Key, g => g.First().Value, StringComparer.Ordinal);

        return new Result<IReadOnlyDictionary<string, string?>>(HttpStatusCode.OK, values, null);
    }
}
