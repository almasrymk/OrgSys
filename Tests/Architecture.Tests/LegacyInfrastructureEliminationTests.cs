using System.IO;
using Xunit;

namespace Architecture.Tests;

/// <summary>
/// Enforces that the legacy root Infrastructure project (OrgContext, generic Repository/
/// UnitOfWork, migrations, InitialData — evacuated and deleted 2026-09-13, see the removal
/// report in that session) never comes back. Same repo-scanning approach as
/// LegacyDomainEliminationTests (there is no assembly left to reflect over — Infrastructure.csproj
/// no longer exists). No exceptions are accepted here by design.
/// </summary>
public class LegacyInfrastructureEliminationTests
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
    public void LegacyInfrastructureProject_MustNotExistOnDisk()
    {
        var legacyCsproj = Path.Combine(RepoRoot, "Infrastructure", "Infrastructure.csproj");

        Assert.False(File.Exists(legacyCsproj),
            $"The legacy Infrastructure project must stay deleted. Found: {legacyCsproj}");
    }

    [Fact]
    public void SolutionFile_MustNotReference_LegacyInfrastructureProject()
    {
        var slnPath = Path.Combine(RepoRoot, "OrgSys.sln");
        var slnText = File.ReadAllText(slnPath);

        Assert.DoesNotContain("Infrastructure\\Infrastructure.csproj", slnText);
        Assert.DoesNotContain("Infrastructure/Infrastructure.csproj", slnText);
    }

    [Fact]
    public void NoCsproj_MustReference_LegacyInfrastructureProject()
    {
        var offenders = SourceFiles("*.csproj")
            .Where(f => File.ReadAllText(f).Contains("\\Infrastructure\\Infrastructure.csproj")
                     || File.ReadAllText(f).Contains("/Infrastructure/Infrastructure.csproj"))
            .ToList();

        Assert.True(offenders.Count == 0,
            $"No project may reference the legacy Infrastructure.csproj. Offending files: {string.Join(", ", offenders)}");
    }

    // Matches `using Infrastructure.X` / `global using Infrastructure.X` / `@using Infrastructure.X`
    // — the legacy root namespace — while deliberately not matching `Sales.Infrastructure`,
    // `Accounting.Infrastructure`, etc. (module Infrastructure namespaces, which are legitimate).
    // The boundary before "Infrastructure" must be a using/space (not a '.'), and it must be
    // followed by '.', ';', or whitespace, never by another identifier character.
    private static readonly System.Text.RegularExpressions.Regex LegacyInfrastructureUsing =
        new(@"(?<![.\w])using\s+Infrastructure(?:\.[A-Za-z0-9_.]*)?\s*[;\r\n]", System.Text.RegularExpressions.RegexOptions.Compiled);

    [Fact]
    public void NoSourceFile_MustImport_LegacyInfrastructureNamespace()
    {
        var offenders = new List<string>();
        var selfPath = Path.Combine(RepoRoot, "Tests", "Architecture.Tests", "LegacyInfrastructureEliminationTests.cs");

        foreach (var f in SourceFiles("*.cs").Concat(SourceFiles("*.cshtml")))
        {
            if (string.Equals(f, selfPath, StringComparison.OrdinalIgnoreCase))
                continue; // this file's own doc comments mention the pattern by name

            var text = File.ReadAllText(f);
            if (LegacyInfrastructureUsing.IsMatch(text))
                offenders.Add(f);
        }

        Assert.True(offenders.Count == 0,
            $"No file may import the legacy 'Infrastructure' namespace (Infrastructure.Persistence.*/Infrastructure.Seed — evacuated to BuildingBlocks/OrgSys.Infrastructure, BuildingBlocks/OrgSys.DatabaseMigrator, and each module's own *.Infrastructure/Seeding). " +
            $"Offending files: {string.Join(", ", offenders)}");
    }
}
