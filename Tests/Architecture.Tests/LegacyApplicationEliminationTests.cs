using System.IO;
using Xunit;

namespace Architecture.Tests;

/// <summary>
/// Enforces that the legacy root Application project (InvoiceJournalIntegration,
/// TransactionJournalIntegration, MappingProfile, GlobalUsings — evacuated and deleted
/// 2026-09-13, see the removal report in that session) never comes back. Same repo-scanning
/// approach as LegacyDomainEliminationTests/LegacyInfrastructureEliminationTests (there is no
/// assembly left to reflect over — Application.csproj no longer exists). No exceptions are
/// accepted here by design.
/// </summary>
public class LegacyApplicationEliminationTests
{
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

    private static IEnumerable<string> SourceFiles(string extension)
    {
        var root = RepoRoot;
        return Directory.EnumerateFiles(root, extension, SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
                     && !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                     && !f.Contains($"{Path.DirectorySeparatorChar}.claude{Path.DirectorySeparatorChar}")
                     && !f.Contains($"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}"));
    }

    [Fact]
    public void LegacyApplicationProject_MustNotExistOnDisk()
    {
        var legacyCsproj = Path.Combine(RepoRoot, "Application", "Application.csproj");

        Assert.False(File.Exists(legacyCsproj),
            $"The legacy Application project must stay deleted. Found: {legacyCsproj}");
    }

    [Fact]
    public void SolutionFile_MustNotReference_LegacyApplicationProject()
    {
        var slnPath = Path.Combine(RepoRoot, "OrgSys.sln");
        var slnText = File.ReadAllText(slnPath);

        Assert.DoesNotContain("Application\\Application.csproj", slnText);
        Assert.DoesNotContain("Application/Application.csproj", slnText);
    }

    [Fact]
    public void NoCsproj_MustReference_LegacyApplicationProject()
    {
        var offenders = SourceFiles("*.csproj")
            .Where(f => File.ReadAllText(f).Contains("\\Application\\Application.csproj")
                     || File.ReadAllText(f).Contains("/Application/Application.csproj"))
            .ToList();

        Assert.True(offenders.Count == 0,
            $"No project may reference the legacy Application.csproj. Offending files: {string.Join(", ", offenders)}");
    }

    // Matches `using Application.X` / `global using Application.X` / bare `using Application;`
    // — the legacy root namespace (Application.Commands.Org.Financials.Integration.*,
    // Application.Mappings) — while deliberately not matching `Sales.Application`,
    // `Accounting.Application`, etc. (module Application namespaces, which are legitimate).
    // The boundary before "Application" must be a using/space (not a '.'), and it must be
    // followed by '.', ';', or whitespace, never by another identifier character.
    private static readonly System.Text.RegularExpressions.Regex LegacyApplicationUsing =
        new(@"(?<![.\w])using\s+Application(?:\.[A-Za-z0-9_.]*)?\s*[;\r\n]", System.Text.RegularExpressions.RegexOptions.Compiled);

    [Fact]
    public void NoSourceFile_MustImport_LegacyApplicationNamespace()
    {
        var offenders = new List<string>();
        var selfPath = Path.Combine(RepoRoot, "Tests", "Architecture.Tests", "LegacyApplicationEliminationTests.cs");

        foreach (var f in SourceFiles("*.cs").Concat(SourceFiles("*.cshtml")))
        {
            if (string.Equals(f, selfPath, StringComparison.OrdinalIgnoreCase))
                continue; // this file's own doc comments mention the pattern by name

            var text = File.ReadAllText(f);
            if (LegacyApplicationUsing.IsMatch(text))
                offenders.Add(f);
        }

        Assert.True(offenders.Count == 0,
            $"No file may import the legacy 'Application' namespace (Application.Commands.Org.Financials.Integration.*/Application.Mappings — evacuated to Accounting.Contracts/Accounting.Application, CommercialDocuments.Application, and Inventory.Application). " +
            $"Offending files: {string.Join(", ", offenders)}");
    }

    [Fact]
    public void NoSourceFile_MustReference_LegacyIntegrationBridgeClasses()
    {
        var offenders = new List<string>();
        var selfPath = Path.Combine(RepoRoot, "Tests", "Architecture.Tests", "LegacyApplicationEliminationTests.cs");

        foreach (var f in SourceFiles("*.cs"))
        {
            if (string.Equals(f, selfPath, StringComparison.OrdinalIgnoreCase))
                continue;

            var text = File.ReadAllText(f);
            if (text.Contains("new InvoiceJournalIntegration(") || text.Contains("new TransactionJournalIntegration("))
                offenders.Add(f);
        }

        Assert.True(offenders.Count == 0,
            $"InvoiceJournalIntegration/TransactionJournalIntegration were replaced by " +
            $"CommercialDocuments.Application.Invoices.Integration.InvoiceJournalPostingService and " +
            $"Inventory.Application.Transactions.Integration.TransactionJournalPostingService (routed through " +
            $"Accounting.Contracts). Offending files: {string.Join(", ", offenders)}");
    }
}
