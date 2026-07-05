using ERP.BuildingBlocks.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.BaseEntities.Identity;

namespace ERP.Infrastructure.DBConfiguration.Config.Identity;

public static class IdentitySchemaDbConfig
{
    public static void ApplyIdentitySchema(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>().ToTable("AspNetUsers", DbSchemaNames.Identity);
        builder.Entity<IdentityRole>().ToTable("AspNetRoles", DbSchemaNames.Identity);
        builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles", DbSchemaNames.Identity);
        builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims", DbSchemaNames.Identity);
        builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins", DbSchemaNames.Identity);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims", DbSchemaNames.Identity);
        builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens", DbSchemaNames.Identity);
    }
}
