using System.IO;
using Xunit;

namespace Architecture.Tests;

/// <summary>
/// Enforces that the legacy root Domain project (fully evacuated and deleted 2026-09-13 — see
/// docs/modular-monolith-analysis.md and the removal report in that session) never comes back,
/// whether as a project reference, a solution entry, or a source-level namespace import. Unlike
/// the NetArchTest-based checks elsewhere in this file set, there is no assembly to reflect over
/// anymore — Domain.csproj no longer exists — so this walks the repository source tree directly.
/// No exceptions are accepted here by design (brief: "Do not add exceptions").
/// </summary>
public class LegacyDomainEliminationTests
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
    public void LegacyDomainProject_MustNotExistOnDisk()
    {
        var legacyCsproj = Path.Combine(RepoRoot, "Domain", "Domain.csproj");

        Assert.False(File.Exists(legacyCsproj),
            $"The legacy Domain project must stay deleted. Found: {legacyCsproj}");
    }

    [Fact]
    public void SolutionFile_MustNotReference_LegacyDomainProject()
    {
        var slnPath = Path.Combine(RepoRoot, "OrgSys.sln");
        var slnText = File.ReadAllText(slnPath);

        Assert.DoesNotContain("Domain\\Domain.csproj", slnText);
        Assert.DoesNotContain("Domain/Domain.csproj", slnText);
    }

    [Fact]
    public void NoCsproj_MustReference_LegacyDomainProject()
    {
        var offenders = SourceFiles("*.csproj")
            .Where(f => File.ReadAllText(f).Contains("\\Domain\\Domain.csproj")
                     || File.ReadAllText(f).Contains("/Domain/Domain.csproj"))
            .ToList();

        Assert.True(offenders.Count == 0,
            $"No project may reference the legacy Domain.csproj. Offending files: {string.Join(", ", offenders)}");
    }

    // Matches `using Domain.X` / `using Domain;` / `global using Domain.X` / `@using Domain.X` —
    // the legacy root namespace — while deliberately not matching `Sales.Domain`, `Purchasing.Domain`,
    // etc. (module Domain namespaces, which are legitimate and unrelated). The boundary before
    // "Domain" must be a using/space (not a '.'), and it must be followed by '.', ';', or whitespace,
    // never by another identifier character (so "Domain2" or "DomainEvents" don't false-positive).
    private static readonly System.Text.RegularExpressions.Regex LegacyDomainUsing =
        new(@"(?<![.\w])using\s+Domain(?:\.[A-Za-z0-9_.]*)?\s*[;\r\n]", System.Text.RegularExpressions.RegexOptions.Compiled);

    [Fact]
    public void NoSourceFile_MustImport_LegacyDomainNamespace()
    {
        var offenders = new List<string>();
        var selfPath = Path.Combine(RepoRoot, "Tests", "Architecture.Tests", "LegacyDomainEliminationTests.cs");

        foreach (var f in SourceFiles("*.cs").Concat(SourceFiles("*.cshtml")))
        {
            if (string.Equals(f, selfPath, StringComparison.OrdinalIgnoreCase))
                continue; // this file's own doc comments/regex mention the pattern by name

            var text = File.ReadAllText(f);
            if (LegacyDomainUsing.IsMatch(text))
                offenders.Add(f);
        }

        Assert.True(offenders.Count == 0,
            $"No file may import the legacy 'Domain' namespace (Domain.Entities/Domain.Enums/Domain.Abstraction/etc. — all evacuated). " +
            $"Offending files: {string.Join(", ", offenders)}");
    }
}
