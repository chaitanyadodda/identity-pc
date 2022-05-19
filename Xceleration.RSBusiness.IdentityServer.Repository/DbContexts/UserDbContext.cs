using Microsoft.EntityFrameworkCore;
using Xceleration.RSBusiness.IdentityServer.Stores.Entities;
using Xceleration.RSBusiness.IdentityServer.Stores.Extensions;

namespace Xceleration.RSBusiness.IdentityServer.Stores.DbContexts;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public virtual DbSet<MemberMaster> MemberMasters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureUserContext();

        base.OnModelCreating(modelBuilder);
    }
}