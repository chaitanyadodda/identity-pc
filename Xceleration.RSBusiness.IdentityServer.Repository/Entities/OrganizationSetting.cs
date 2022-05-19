namespace Xceleration.RSBusiness.IdentityServer.Stores.Entities;

public class OrganizationSetting
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public List<ClientOrganizationSetting> ClientOrganizationSettings { get; set; }
}