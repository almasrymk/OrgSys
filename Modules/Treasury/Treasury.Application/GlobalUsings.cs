global using OrgSys.SharedKernel;
global using Treasury.Domain;
global using Treasury.Application;
global using MasterData.Domain;
global using Accounting.Domain;
global using Accounting.Application;
global using CommercialDocuments.Domain;
global using Parties.Domain;
// Preference hasn't been extracted from the monolith yet (docs/modular-monolith-analysis.md
// §13 — stays a generic cross-cutting entity for now). "Domain.Entities" would normally resolve
// here, but every module has a sibling "<Module>.Domain" namespace nested under it
// (Treasury.Domain here), so plain "Domain.Entities" resolves to the (nonexistent)
// "Treasury.Domain.Entities" first per C#'s enclosing-namespace lookup rule. global:: anchors
// it to the true root and bypasses that shadow.
global using global::Domain.Entities;
