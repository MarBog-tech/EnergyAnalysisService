using EnergyAnalysisService.Models.Entity;
using EnergyAnalysisService.Models.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnergyAnalysisService.DataAccess.Context;

public class EnergyAnalysisServiceContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<LocalUser> LocalUsers { get; set; }
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Device>? Devices { get; set; }
    public DbSet<PaymentSlip> PaymentSlips { get; set; }
    public DbSet<Settings>? Settings { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public EnergyAnalysisServiceContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Settings>()
            .Property(s => s.PriceForElectricity)
            .HasDefaultValue(2.64m);
        
        base.OnModelCreating(builder);
    }
}