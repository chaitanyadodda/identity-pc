using Microsoft.EntityFrameworkCore;
using Xceleration.RSBusiness.IdentityServer.Contracts;
using Xceleration.RSBusiness.IdentityServer.Stores.Entities;

namespace Xceleration.RSBusiness.IdentityServer.Stores.Extensions;

public static class ModelBuilderExtensions
{
    public static void ConfigurePcConfigurationContext(this ModelBuilder builder)
    {
        builder.Entity<OrganizationSetting>(org =>
        {
            org.ToTable(Constants.Database.StoreTableNames.OrganizationSetting);
            org.HasKey(x => x.Id);
        });
        builder.Entity<ClientOrganizationSetting>(org =>
        {
            org.ToTable(Constants.Database.StoreTableNames.ClientOrganizationSetting);
            org.HasKey(x => x.ClientId);
        });
    }

    public static void ConfigureUserContext(this ModelBuilder builder)
    {
        builder.Entity<LoginMaster>(login =>
        {
            login.ToTable(Constants.Database.StoreTableNames.LoginMaster);
            login.Property(x => x.WebUserId).HasMaxLength(100);
            login.Property(x => x.WebPassword).HasMaxLength(50);
            login.Property(x => x.EntityName).HasMaxLength(165);
            login.HasKey(x => x.LoginId);
        });

        builder.Entity<MemberDetail>(details =>
        {
            details.ToTable(Constants.Database.StoreTableNames.MemberDetails);
            details.HasKey(x => new { x.MemberId, x.CorporateAccountId });
        });

        builder.Entity<MemberMaster>(master =>
        {
            master.ToTable(Constants.Database.StoreTableNames.MemberMaster);
            master.HasKey(x => x.MemberId);
            master.HasOne(x => x.MemberDetails)
                .WithOne(x => x.MemberMaster)
                .HasPrincipalKey<MemberDetail>(x => x.MemberId)
                .HasForeignKey<MemberMaster>(x => x.MemberId);
            master.HasOne(x => x.LoginMaster)
                .WithOne(x => x.MemberMaster)
                .HasPrincipalKey<LoginMaster>(x => x.WebUserId)
                .HasForeignKey<MemberMaster>(x => x.EmailId);
            master.HasOne(x => x.AddressMaster)
                .WithOne(x => x.MemberMaster)
                .HasPrincipalKey<MemberMaster>(x => x.AddressId)
                .HasForeignKey<AddressMaster>(x => x.AddressId);
        });

        builder.Entity<AddressMaster>(address =>
        {
            address.ToTable(Constants.Database.StoreTableNames.AddressMaster);
            address.HasKey(x => x.AddressId);
            address.HasOne(x => x.CityMaster)
                .WithMany(x => x.AddressMasters)
                .HasForeignKey(x => x.CityId);
            address.HasOne(x => x.DistrictMaster)
                .WithMany(x => x.AddressMasters)
                .HasForeignKey(x => x.DistrictId);
            address.HasOne(x => x.CountryMaster)
                .WithMany(x => x.AddressMasters)
                .HasForeignKey(x => x.CountryId);
        });
        builder.Entity<CityMaster>(city =>
        {
            city.ToTable(Constants.Database.StoreTableNames.CityMaster);
            city.HasKey(x => x.CityId);
            city.HasOne(x => x.StateMaster)
                .WithMany(x => x.CityMasters)
                .HasForeignKey(x => x.StateId);
            city.HasOne(x => x.DistrictMaster)
                .WithMany(x => x.CityMasters)
                .HasForeignKey(x => x.DistrictId);
        });
        builder.Entity<CountryMaster>(cntry =>
        {
            cntry.ToTable(Constants.Database.StoreTableNames.CountryMaster);
            cntry.HasKey(x => x.CountryId);
        });
        builder.Entity<DistrictMaster>(district =>
        {
            district.ToTable(Constants.Database.StoreTableNames.DistrictMaster);
            district.HasKey(x => x.DistrictId);
        });
        builder.Entity<StateMaster>(state =>
        {
            state.ToTable(Constants.Database.StoreTableNames.StateMaster);
            state.HasKey(x => x.StateId);
            state.HasOne(x => x.CountryMaster)
                .WithMany(x => x.StateMasters)
                .HasForeignKey(x => x.CountryId);
        });
    }
}