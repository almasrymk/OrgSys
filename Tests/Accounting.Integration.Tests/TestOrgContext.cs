namespace Accounting.Integration.Tests;

using Accounting.Domain;
using Administration.Domain;
using CommercialDocuments.Domain;
using Microsoft.EntityFrameworkCore;
using OrgSys.SharedKernel;
using Parties.Domain;

/// <summary>
/// SQLite-backed IOrgContext covering the invoice → journal posting graph (Journal aggregate,
/// Account, Preference, Dealer, Invoice). Cross-module navigations are ignored so this project
/// does not need SQL Server-only artifacts or the full production OrgContext.
/// </summary>
public sealed class TestOrgContext(DbContextOptions<TestOrgContext> options) : DbContext(options), IOrgContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Journal>(b =>
        {
            b.Ignore(e => e.JournalType);
            b.Ignore(e => e.FiscalYear);
            b.Ignore(e => e.FiscalPeriod);
            b.Ignore(e => e.OriginalJournal);
            b.Ignore(e => e.ReversalJournal);
            b.Ignore(e => e.DomainEvents);
            b.HasMany<JournalItem>("JournalItems")
                .WithOne(i => i.Journal)
                .HasForeignKey(i => i.JournalId);
            b.Navigation("JournalItems").HasField("_journalItems").UsePropertyAccessMode(PropertyAccessMode.Field);
            b.Property(e => e.Rate).HasPrecision(18, 2);
        });

        modelBuilder.Entity<JournalItem>(b =>
        {
            b.Ignore(e => e.Account);
            b.Property(e => e.Debit).HasPrecision(18, 2);
            b.Property(e => e.Credit).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Account>(b =>
        {
            b.Ignore(e => e.AccountType);
            b.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Preference>(b =>
        {
            b.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Dealer>(b =>
        {
            b.Ignore(e => e.CustomerProfile);
            b.Ignore(e => e.SupplierProfile);
            b.Ignore(e => e.Contacts);
            b.Ignore(e => e.Addresses);
            b.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Invoice>(b =>
        {
            b.Ignore(e => e.InvoiceProducts);
            b.Property(e => e.Rate).HasPrecision(18, 2);
            b.Property(e => e.Net).HasPrecision(18, 2);
            b.Property(e => e.Total).HasPrecision(18, 2);
            b.Property(e => e.Discount).HasPrecision(18, 2);
            b.Property(e => e.Tax).HasPrecision(18, 2);
        });
    }

    public void ResetDbContextState()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
    }

    public Task BeginTransactionAsync() => Database.BeginTransactionAsync();

    public Task CommitAsync() => Database.CommitTransactionAsync();

    public Task RollbackAsync() => Database.RollbackTransactionAsync();
}
