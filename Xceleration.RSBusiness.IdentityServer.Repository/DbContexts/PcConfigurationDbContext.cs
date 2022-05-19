using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Xceleration.RSBusiness.IdentityServer.Stores.Entities;
using Xceleration.RSBusiness.IdentityServer.Stores.Extensions;

namespace Xceleration.RSBusiness.IdentityServer.Stores.DbContexts;

public class PcConfigurationDbContext : ConfigurationDbContext<PcConfigurationDbContext>
{
    public PcConfigurationDbContext(DbContextOptions<PcConfigurationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<OrganizationSetting> OrganizationSettings { get; set; }
    public virtual DbSet<ClientOrganizationSetting> ClientOrganizationSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigurePcConfigurationContext();

        base.OnModelCreating(modelBuilder);
    }
}