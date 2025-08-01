﻿using ERP.Application.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.BaseEntities;
using Shared.BaseEntities.Identity;

namespace Domain.Account.DBConfiguration.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> , IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    protected void ApplyCreateUpdateTime()
    {
        var entries = ChangeTracker
        .Entries()
        .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Modified)
                ((BaseEntity)entityEntry.Entity).ModifiedAt = DateTime.Now;
            else if (entityEntry.State == EntityState.Added)
                ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
        }
    }
    public override int SaveChanges()
    {
        ApplyCreateUpdateTime();
        return base.SaveChanges();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyCreateUpdateTime();
        return base.SaveChangesAsync(cancellationToken);
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyCreateUpdateTime();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyCreateUpdateTime();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}