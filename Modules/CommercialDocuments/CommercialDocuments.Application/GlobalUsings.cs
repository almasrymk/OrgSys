global using OrgSys.SharedKernel;
global using CommercialDocuments.Domain;
global using CommercialDocuments.Application;
global using CommercialDocuments.Contracts;
global using Parties.Domain;
global using MasterData.Domain;
global using MasterData.Application;
global using Accounting.Domain;
global using Organization.Domain;
// Preference and the Financial/Journal integration bridges haven't been extracted from the
// monolith yet (docs/modular-monolith-analysis.md §21) — moved unchanged from Sales.Application.
// global:: anchors "Domain.Entities" to the true root, bypassing the nested-namespace shadow
// every module hits (CommercialDocuments.Domain sits directly under "CommercialDocuments" too).
global using global::Domain.Entities;
