global using OrgSys.SharedKernel;
global using Sales.Domain;
global using Sales.Application;
global using Parties.Domain;
global using MasterData.Domain;
global using MasterData.Application;
global using Accounting.Domain;
global using Organization.Domain;
// Preference and the Financial/Journal integration bridges haven't been extracted from the
// monolith yet (docs/modular-monolith-analysis.md §21). global:: anchors "Domain.Entities" to
// the true root, bypassing the nested-namespace shadow every module hits (see
// docs/modular-monolith-target-architecture.md — Sales.Domain sits directly under "Sales" too).
global using global::Domain.Entities;
