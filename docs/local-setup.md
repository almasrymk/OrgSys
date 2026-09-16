# Local and deployed configuration

Tracked `appsettings.json` / `appsettings.Development.json` files contain **placeholders only**.
Real connection strings and signing keys must never be committed.

## Required secrets

| Setting | Environment variable | Purpose |
|---|---|---|
| `ConnectionStrings:OrgConnection` | `ConnectionStrings__OrgConnection` | SQL Server connection for the OrgSys database |
| `Jwt:Key` | `Jwt__Key` | HMAC signing key for API JWTs (use a long random value, 32+ bytes) |

`Jwt:Issuer`, `Jwt:Audience`, and `Jwt:ExpiryMinutes` are not secrets and may stay in tracked config.

## Local development

Prefer [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) so values never hit disk in the repo:

```bash
dotnet user-secrets set "ConnectionStrings:OrgConnection" "Data Source=.;Initial Catalog=ORGDb;User Id=sa;Password=...;Encrypt=True;TrustServerCertificate=True;" --project API
dotnet user-secrets set "Jwt:Key" "<long-random-key>" --project API
```

The legacy MVC host uses the same names if you still run `OrgSys/`:

```bash
dotnet user-secrets set "ConnectionStrings:OrgConnection" "Data Source=.;Initial Catalog=ORGDb;User Id=sa;Password=...;Encrypt=True;TrustServerCertificate=True;" --project OrgSys
```

Alternatively, copy the values into an untracked local file (gitignored):

- `API/appsettings.Local.json`
- `OrgSys/appsettings.Local.json`
- any `appsettings.*.Local.json`

Example `appsettings.Local.json`:

```json
{
  "ConnectionStrings": {
    "OrgConnection": "Data Source=.;Initial Catalog=ORGDb;User Id=sa;Password=...;Encrypt=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "<long-random-key>"
  }
}
```

## Deployed environments

Supply the same keys via environment variables or the host's secret manager:

- `ConnectionStrings__OrgConnection`
- `Jwt__Key`

Do not put live values in tracked `appsettings*.json`. Nested configuration uses a double underscore (`__`) as the environment-variable separator.

## Design-time EF Core

`dotnet ef` reads the same configuration. Set `ConnectionStrings__OrgConnection` (or user-secrets / `appsettings.Local.json`) before `dotnet ef migrations add` / `database update`. The API host refuses to start if `OrgConnection` or `Jwt:Key` are still placeholders (except at EF design time).
