# OrgSys.Angular

Angular frontend for OrgSys ERP, consuming the existing `API` project. Presentation-only: business rules, validation, and persistence stay in `Domain`/`Application`/`Infrastructure`/`API`. See [../docs/ANGULAR_MIGRATION_INVENTORY.md](../docs/ANGULAR_MIGRATION_INVENTORY.md) for the full migration plan and status.

## Prerequisites

- Node.js 22.x
- The `API` project running on `https://localhost:44300` (its `launchSettings.json` default). CORS is configured there for `http://localhost:4200`/`https://localhost:4200`.

## Run

```bash
npm install
npm start
```

Serves at `http://localhost:4200`. Login with a seeded user (e.g. `Owner` / `P@ssw0rd`).

## Structure

Feature-based (`core/`, `shared/`, `layout/`, `features/`) — see the migration inventory doc for the rationale and the full target layout. `features/administration/countries` is the reference implementation: copy its shape (`models/`, `services/`, `pages/`, `*.routes.ts`) for each new feature.

## Auth

JWT bearer, issued by `POST /api/Auth/login`. `core/auth/auth.service.ts` stores the token; `core/interceptors/auth.interceptor.ts` attaches it to every API request. `core/auth/permission.service.ts` + `*appHasPermission` replace OrgSys.App's `User.IsAllowed(...)`.
