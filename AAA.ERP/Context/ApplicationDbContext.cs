namespace AAA.ERP;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id =Guid.NewGuid().ToString(),
                Name = SD.Role_Admin,
                NormalizedName = SD.Role_Admin.ToUpper(),
            }
            );
    }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}