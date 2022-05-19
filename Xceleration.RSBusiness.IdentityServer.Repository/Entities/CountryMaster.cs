namespace Xceleration.RSBusiness.IdentityServer.Stores.Entities;

public class CountryMaster
{
    public int CountryId { get; set; }
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public IEnumerable<AddressMaster> AddressMasters { get; set; }
    public IEnumerable<StateMaster> StateMasters { get; set; }
}