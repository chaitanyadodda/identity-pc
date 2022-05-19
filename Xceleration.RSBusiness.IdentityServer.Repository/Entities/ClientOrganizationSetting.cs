using Duende.IdentityServer.EntityFramework.Entities;

namespace Xceleration.RSBusiness.IdentityServer.Stores.Entities;

public class ClientOrganizationSetting
{
    public int ClientId { get; set; }
    public int OrganizationSettingId { get; set; }

    public OrganizationSetting OrganizationSetting { get; set; }
    public Client Client { get; set; }
}