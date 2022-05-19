namespace Xceleration.RSBusiness.IdentityServer.Stores.Entities;

public class StateMaster
{
    public int StateId { get; set; }
    public string StateName { get; set; }
    public IEnumerable<CityMaster> CityMasters { get; set; }
    public int CountryId { get; set; }
    public CountryMaster CountryMaster { get; set; }
}