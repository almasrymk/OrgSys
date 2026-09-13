// Modules being extracted from this monolith move their base entity types out of Domain.Entities.
// A global using here means the hundreds of files that rely on the bare (non-qualified) type
// name — e.g. `BaseModel`, `Status` — keep compiling without per-file edits as each module's
// types leave the old Domain project. See docs/modular-monolith-analysis.md §20/§21 and
// docs/modular-monolith-target-architecture.md §2.
global using OrgSys.SharedKernel;
global using MasterData.Domain;
global using MasterData.Application;
global using Organization.Domain;
global using Organization.Application;
global using Administration.Domain;
global using Administration.Application;
global using Accounting.Domain;
global using Accounting.Application;
global using Treasury.Domain;
global using Treasury.Application;
global using CommercialDocuments.Domain;
global using Parties.Domain;
global using Inventory.Domain;
