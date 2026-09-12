global using OrgSys.SharedKernel;
global using Inventory.Domain;
global using Inventory.Application;
global using MasterData.Domain;
global using MasterData.Application;
global using Accounting.Domain;
global using Organization.Domain;
global using Sales.Domain;
global using Parties.Domain;
// Preference and the Financial/Journal integration bridges haven't been extracted from the
// monolith yet (docs/modular-monolith-analysis.md §21). global:: anchors "Domain.Entities" to
// the true root, bypassing the nested-namespace shadow every module hits.
global using global::Domain.Entities;
