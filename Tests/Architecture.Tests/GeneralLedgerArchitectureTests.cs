using System.IO;
using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace Architecture.Tests;

/// <summary>
/// GeneralLedger (Accounting module) DDD migration — Phase 1 completion checklist (see the
/// migration report). Most cross-module isolation is already exercised generically by
/// ModuleDependencyTests/ModuleLayerDependencyTests/ModuleInfrastructureDependencyTests (Accounting
/// is one row of their module matrices); this file adds the checks specific to this phase:
/// Accounting.Domain -> API, and that Journal's aggregate boundary (Status/Posted/JournalItems) is
/// only ever mutated from inside Journal itself, not reached around via direct field assignment or
/// raw collection mutation elsewhere in the module.
/// </summary>
public class GeneralLedgerArchitectureTests
{
    private static readonly Assembly AccountingDomainAssembly = typeof(Accounting.Domain.AssemblyMarker).Assembly;

    [Fact]
    public void AccountingDomain_MustNotDependOn_AccountingApplication()
    {
        var result = Types.InAssembly(AccountingDomainAssembly)
            .Should()
            .NotHaveDependencyOn("Accounting.Application")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Accounting.Domain (GeneralLedger) must not depend on Accounting.Application. Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void AccountingDomain_MustNotDependOn_AccountingInfrastructure()
    {
        var result = Types.InAssembly(AccountingDomainAssembly)
            .Should()
            .NotHaveDependencyOn("Accounting.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Accounting.Domain (GeneralLedger) must not depend on Accounting.Infrastructure. Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void AccountingDomain_MustNotDependOn_Api()
    {
        // Only "API" is checked here, not the "OrgSys" MVC host — its own root namespace is
        // literally "OrgSys", which NetArchTest's namespace-prefix matching cannot distinguish from
        // the legitimate BuildingBlocks it shares a prefix with (OrgSys.SharedKernel,
        // OrgSys.EventBus, ...), which Accounting.Domain is allowed and expected to depend on.
        var result = Types.InAssembly(AccountingDomainAssembly)
            .Should()
            .NotHaveDependencyOn("API")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Accounting.Domain (GeneralLedger) must not depend on the API host project. Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void AccountingDomain_MustNotDependOn_MediatROrEfCore()
    {
        var result = Types.InAssembly(AccountingDomainAssembly)
            .Should()
            .NotHaveDependencyOnAny("MediatR", "Microsoft.EntityFrameworkCore")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Accounting.Domain (GeneralLedger) must stay framework-agnostic. Offending types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    // ----- Journal aggregate encapsulation (repo-scan) -----
    // NetArchTest can prove assembly-level dependency absence but not "who calls this setter" —
    // that needs a source scan, same technique as the Legacy*EliminationTests.

    private static string RepoRoot
    {
        get
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "OrgSys.sln")))
                dir = dir.Parent;

            return dir?.FullName
                ?? throw new InvalidOperationException("Could not locate OrgSys.sln above " + AppContext.BaseDirectory);
        }
    }

    private static IEnumerable<string> AccountingSourceFiles(string excludeFileName)
    {
        var accountingRoot = Path.Combine(RepoRoot, "Modules", "Accounting");
        return Directory.EnumerateFiles(accountingRoot, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
                     && !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                     && !Path.GetFileName(f).Equals(excludeFileName, StringComparison.OrdinalIgnoreCase));
    }

    // Matches `journal.Status = ...` / `.Posted = ...` assignment on some variable/property named
    // *journal*-ish — deliberately narrow (word "journal" case-insensitive right before the dot) so
    // it doesn't flag unrelated Status/Posted fields on other MovementModel-derived entities
    // (Financial, Invoice, Transaction, ...) elsewhere in the solution; this test only scans
    // Modules/Accounting anyway, where "journal"-named variables are unambiguous.
    private static readonly System.Text.RegularExpressions.Regex DirectStatusOrPostedAssignment =
        new(@"\bjournal(s)?\b\.(Status|Posted)\s*=(?!=)", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);

    private static readonly System.Text.RegularExpressions.Regex DirectJournalItemsCollectionMutation =
        new(@"\.JournalItems\.(Add|AddRange|Remove|RemoveAll|Clear|Insert)\s*\(", System.Text.RegularExpressions.RegexOptions.Compiled);

    [Fact]
    public void NoCodeOutsideJournal_DirectlyAssignsJournalStatusOrPosted()
    {
        var offenders = new List<string>();

        foreach (var file in AccountingSourceFiles(excludeFileName: "Journal.cs"))
        {
            var text = File.ReadAllText(file);
            if (DirectStatusOrPostedAssignment.IsMatch(text))
                offenders.Add(file);
        }

        Assert.True(offenders.Count == 0,
            "Journal.Status/Posted must only be mutated by Journal's own behavior methods " +
            "(Post/Cancel/Redo/CreateReversal/SyncStatusFromSourceDocument) — never by direct " +
            $"property assignment elsewhere. Offending files: {string.Join(", ", offenders)}");
    }

    [Fact]
    public void NoCodeOutsideJournal_DirectlyMutatesJournalItemsCollection()
    {
        var offenders = new List<string>();

        foreach (var file in AccountingSourceFiles(excludeFileName: "Journal.cs"))
        {
            var text = File.ReadAllText(file);
            if (DirectJournalItemsCollectionMutation.IsMatch(text))
                offenders.Add(file);
        }

        Assert.True(offenders.Count == 0,
            "JournalItem lines must only be added/removed through Journal.AddLine/UpdateLine/" +
            "RemoveLine/ReplaceLinesFromSourceDocument — never by mutating the JournalItems " +
            $"collection directly. Offending files: {string.Join(", ", offenders)}");
    }
}
